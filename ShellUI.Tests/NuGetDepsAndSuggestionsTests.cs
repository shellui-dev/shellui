using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class RegistrySuggestionsTests
{
    [Theory]
    [InlineData("datatable", "data-table")]
    [InlineData("data_table", "data-table")]
    [InlineData("buttong", "button")]
    [InlineData("chrt", "chart")]
    [InlineData("themetoggle", "theme-toggle")]
    public void FindClosestMatch_SuggestsExpectedComponent(string typo, string expected)
    {
        Assert.Equal(expected, ComponentRegistry.FindClosestMatch(typo));
    }

    [Fact]
    public void FindClosestMatch_ReturnsNullForExactMatch()
    {
        // Exact matches have distance 0 and are excluded from suggestions —
        // the caller already knows the component exists.
        Assert.Null(ComponentRegistry.FindClosestMatch("button"));
    }

    [Fact]
    public void FindClosestMatch_ReturnsNullWhenNothingIsClose()
    {
        Assert.Null(ComponentRegistry.FindClosestMatch("xyzzy-no-such-component-at-all"));
    }

    [Fact]
    public void FindClosestMatch_DoesNotSuggestHiddenSubComponents()
    {
        // `data-table-models` is IsAvailable=false (installed only as a dep of data-table).
        // A user typo like "data-table-modls" should not be redirected to it.
        var result = ComponentRegistry.FindClosestMatch("data-table-modls");
        Assert.NotEqual("data-table-models", result);
    }
}

public class NuGetDependenciesTests
{
    [Fact]
    public void DataTableModels_IsRegisteredAndHidden()
    {
        var metadata = ComponentRegistry.GetMetadata("data-table-models");
        Assert.NotNull(metadata);
        Assert.False(metadata!.IsAvailable, "data-table-models is a sub-component dep and must not appear in `shellui list`");
    }

    [Fact]
    public void DataTable_DeclaresSystemLinqDynamicCore()
    {
        var metadata = ComponentRegistry.GetMetadata("data-table");
        Assert.NotNull(metadata);
        Assert.Contains(metadata!.NuGetDependencies, p => p.PackageId == "System.Linq.Dynamic.Core");
    }

    [Fact]
    public void Chart_DeclaresBlazorApexCharts()
    {
        var metadata = ComponentRegistry.GetMetadata("chart");
        Assert.NotNull(metadata);
        Assert.Contains(metadata!.NuGetDependencies, p => p.PackageId == "Blazor-ApexCharts");
    }

    [Theory]
    [InlineData("pie-chart")]
    [InlineData("donut-chart")]
    [InlineData("radar-chart")]
    [InlineData("radial-chart")]
    [InlineData("bar-chart")]
    [InlineData("area-chart")]
    [InlineData("line-chart")]
    [InlineData("multi-series-chart")]
    public void ChartVariants_TransitivelyPullInBlazorApexCharts(string componentName)
    {
        // Chart family components don't declare the NuGet dep themselves — they depend
        // on `chart` which does. Verify the dependency chain is intact so the installer's
        // recursive walk picks up the package.
        var metadata = ComponentRegistry.GetMetadata(componentName);
        Assert.NotNull(metadata);
        Assert.Contains("chart", metadata!.Dependencies);
    }
}

public class DependencyGraphTests
{
    // Every dependency name declared by any registered component must itself resolve
    // via ComponentRegistry.GetMetadata. Catches the class of bug where a sub-component
    // template exists on disk but is never wired into the registry — the CLI then
    // reports "Failed: <dep-name>" and the consumer project won't compile.
    [Fact]
    public void EveryDeclaredDependency_ResolvesInRegistry()
    {
        var missing = new List<string>();
        foreach (var (name, metadata) in ComponentRegistry.Components)
        {
            foreach (var dep in metadata.Dependencies)
            {
                if (ComponentRegistry.GetMetadata(dep) is null)
                    missing.Add($"{name} -> {dep}");
            }
        }
        Assert.True(missing.Count == 0,
            "The following declared dependencies are not registered in ComponentRegistry:\n  " +
            string.Join("\n  ", missing));
    }

    // Every installable component (IsAvailable = true) must route through the
    // GetComponentContent switch. Sub-component stubs registered as IsAvailable = false
    // with intentionally-empty Content are allowed — they exist only so the CLI can
    // resolve them as dependency names when the parent renders their markup inline.
    [Fact]
    public void EveryInstallableComponent_HasContentMapping()
    {
        var missing = new List<string>();
        foreach (var (name, metadata) in ComponentRegistry.Components)
        {
            if (!metadata.IsAvailable) continue;
            var content = ComponentRegistry.GetComponentContent(name);
            if (string.IsNullOrWhiteSpace(content))
                missing.Add(name);
        }
        Assert.True(missing.Count == 0,
            "The following installable components have no content mapping:\n  " +
            string.Join("\n  ", missing));
    }
}

public class HiddenNetworkDependencyTests
{
    // ShellUI never links Google Material Symbols/Icons or Font Awesome and every
    // component uses inline SVG for its chrome. When a template depends on an
    // external icon font the raw icon name ("expand_more", "cloud_upload", ...)
    // shows as literal text or the glyph slot renders empty on consumers.
    // Fail loudly here so the SVG-swap convention stays enforced across the whole
    // registry — the specific classes below cover both Material families and
    // Font Awesome's solid/regular/brands.
    [Theory]
    [InlineData("material-symbols-outlined")]   // Material Symbols (newer variable font)
    [InlineData("material-symbols-rounded")]
    [InlineData("material-symbols-sharp")]
    [InlineData("material-icons")]              // Material Icons (older static font)
    [InlineData("fa-solid")]                    // Font Awesome v6+
    [InlineData("fa-regular")]
    [InlineData("fa-brands")]
    public void NoTemplate_DependsOnExternalIconFont(string cssClass)
    {
        var offenders = new List<string>();
        foreach (var (name, _) in ComponentRegistry.Components)
        {
            var content = ComponentRegistry.GetComponentContent(name);
            if (content is not null && content.Contains(cssClass))
                offenders.Add(name);
        }
        Assert.True(offenders.Count == 0,
            $"The following templates reference `{cssClass}` (external icon font, not shipped by ShellUI):\n  " +
            string.Join("\n  ", offenders));
    }
}

public class RelativeJsModuleImportTests
{
    // A component whose C# does JSRuntime.InvokeAsync("import", "./foo.js") resolves that
    // path against the current page URL. That only works when ShellUI is installed straight
    // into the host app; the moment the generated component is compiled into a consumer's
    // own Razor Class Library, the asset is served from _content/<Library>/ instead and the
    // import 404s — silently, since every one of these calls is wrapped in try/catch.
    // ShellUI's established fix for this shape of bug (see ThemeToggle, InputOTP,
    // CommandPalette, Combobox, ...) is to route through the already-loaded global
    // `window.ShellUI` object (shellui.js, loaded via one host-controlled <script> tag)
    // instead of a per-component dynamic import. Fail loudly if a template reintroduces it.
    [Fact]
    public void NoTemplate_DynamicallyImportsARelativeJsModule()
    {
        var offenders = new List<string>();
        foreach (var (name, _) in ComponentRegistry.Components)
        {
            if (name == "sidebar-js") continue;

            var content = ComponentRegistry.GetComponentContent(name);
            if (content is not null && Regex.IsMatch(content, @"""import""\s*,\s*""\.{1,2}/"))
                offenders.Add(name);
        }
        Assert.True(offenders.Count == 0,
            "The following templates dynamically import a relative JS module (breaks when " +
            "compiled into a consumer's own Razor Class Library — route through window.ShellUI " +
            "in shellui.js instead):\n  " + string.Join("\n  ", offenders));
    }
}

public class SidebarInteropTests
{
    [Fact]
    public void CliSidebarProvider_UsesGlobalLifecycleInterop()
    {
        var content = ComponentRegistry.GetComponentContent("sidebar-provider");

        Assert.NotNull(content);
        Assert.Contains("ShellUI.initSidebar", content!);
        Assert.Contains("ShellUI.disposeSidebar", content!);
        Assert.DoesNotContain("shellui-sidebar.js", content!);
    }

    [Fact]
    public void ShellUiJs_ProvidesSidebarLifecycleInterop()
    {
        var content = ComponentRegistry.GetComponentContent("shellui-js");

        Assert.NotNull(content);
        Assert.Contains("initSidebar: function (handle, dotNetRef)", content!);
        Assert.Contains("disposeSidebar: function (handle)", content!);
        Assert.Contains("this._sidebarHandlers.delete(handle)", content!);
    }

    [Fact]
    public void SidebarJs_IsRetainedOnlyAsAHiddenLegacyAlias()
    {
        var metadata = ComponentRegistry.GetMetadata("sidebar-js");
        var sidebar = ComponentRegistry.GetMetadata("sidebar");
        var provider = ComponentRegistry.GetMetadata("sidebar-provider");

        Assert.NotNull(metadata);
        Assert.False(metadata!.IsAvailable);
        Assert.Equal("../../wwwroot/shellui-sidebar.js", metadata.FilePath);
        Assert.DoesNotContain("sidebar-js", sidebar!.Dependencies);
        Assert.DoesNotContain("sidebar-js", provider!.Dependencies);
        Assert.Contains("shellui-js", provider.Dependencies);
    }

    [Fact]
    public void PackageSidebarProvider_RetainsItsRclStaticAsset()
    {
        var repositoryRoot = FindRepositoryRoot();
        var providerPath = Path.Combine(repositoryRoot, "src", "ShellUI.Components", "Components", "SidebarProvider.razor");
        var assetPath = Path.Combine(repositoryRoot, "src", "ShellUI.Components", "wwwroot", "shellui-sidebar.js");

        var provider = File.ReadAllText(providerPath);
        var asset = File.ReadAllText(assetPath);

        Assert.Contains("./_content/ShellUI.Components/shellui-sidebar.js", provider);
        Assert.DoesNotContain("\"./shellui-sidebar.js\"", provider);
        Assert.Contains("export function initSidebar", asset);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory != null && !File.Exists(Path.Combine(directory.FullName, "ShellUI.slnx")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new InvalidOperationException("Repository root not found.");
    }
}

public class DataTableTemplateContentTests
{
    // The library-wide convention is `Components.Models` for model namespaces regardless
    // of where the file lives on disk (TabModels, StepperModels, ContextMenuModels,
    // ChartModels all use this). The DataTable @using and DataTableModels namespace
    // must agree on that convention so consumers can compile.
    [Fact]
    public void DataTable_UsingDirectiveMatchesDataTableModelsNamespace()
    {
        var dataTable = ComponentRegistry.GetComponentContent("data-table");
        var models = ComponentRegistry.GetComponentContent("data-table-models");

        Assert.NotNull(dataTable);
        Assert.NotNull(models);
        Assert.Contains("@using YourProjectNamespace.Components.Models", dataTable);
        Assert.Contains("namespace YourProjectNamespace.Components.Models;", models);
    }
}
