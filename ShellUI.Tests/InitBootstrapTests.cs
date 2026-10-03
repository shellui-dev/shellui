using ShellUI.CLI.Services;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class InitBootstrapTests
{
    // The net9 `dotnet new blazor` App.razor, including the @Assets[] wrapper.
    private const string FreshAppRazor = @"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <base href=""/"" />
    <link rel=""stylesheet"" href=""@Assets[""app.css""]"" />
    <ImportMap />
    <HeadOutlet />
</head>

<body>
    <Routes />
    <script src=""@Assets[""_framework/blazor.web.js""]""></script>
</body>

</html>
";

    [Theory]
    [InlineData(@"    <link rel=""stylesheet"" href=""@Assets[""lib/bootstrap/dist/css/bootstrap.min.css""]"" />")]
    [InlineData(@"    <link rel=""stylesheet"" href=""bootstrap/bootstrap.min.css"" />")]
    public void RewriteAppRazor_RemovesTheDeletedLocalBootstrapLink(string link)
    {
        var app = FreshAppRazor.Replace("    <link rel=\"stylesheet\" href=\"@Assets[\"app.css\"]\" />", link + "\n    <link rel=\"stylesheet\" href=\"@Assets[\"app.css\"]\" />");

        var result = InitService.RewriteAppRazor(app);

        Assert.DoesNotContain("bootstrap.min.css", result);
        Assert.Contains("@Assets[\"app.css\"]", result);
    }

    [Fact]
    public void RewriteAppRazor_KeepsCdnBootstrap()
    {
        const string cdn = @"<link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"" />";

        Assert.Contains(cdn, InitService.RewriteAppRazor(FreshAppRazor.Replace("<ImportMap />", cdn + "\n    <ImportMap />")));
    }

    [Fact]
    public void RewriteAppRazor_AddsRenderModeToHeadOutletAndRoutes()
    {
        var result = InitService.RewriteAppRazor(FreshAppRazor);

        Assert.Contains(@"<HeadOutlet @rendermode=""InteractiveServer"" />", result);
        Assert.Contains(@"<Routes @rendermode=""InteractiveServer"" />", result);
    }

    [Fact]
    public void RewriteAppRazor_InjectsThemeBootstrapInHead()
    {
        var result = InitService.RewriteAppRazor(FreshAppRazor);

        Assert.Contains("ShellUI theme bootstrap", result);
        Assert.Contains("classList.add('dark')", result);
        var themeIdx = result.IndexOf("ShellUI theme bootstrap");
        var headCloseIdx = result.IndexOf("</head>");
        Assert.True(themeIdx > 0 && themeIdx < headCloseIdx);
    }

    [Fact]
    public void RewriteAppRazor_InjectsShelluiJsBeforeBlazorScript()
    {
        var result = InitService.RewriteAppRazor(FreshAppRazor);

        var shelluiIdx = result.IndexOf(@"<script src=""shellui.js""></script>");
        var blazorIdx = result.IndexOf("blazor.web.js");

        Assert.True(shelluiIdx > 0, "shellui.js script tag was not injected");
        Assert.True(shelluiIdx < blazorIdx, "shellui.js must precede blazor.web.js so window.ShellUI.* is defined before Blazor calls into it");
    }

    [Fact]
    public void RewriteAppRazor_HandlesBareBlazorScriptTag()
    {
        // Older templates use the bare form without @Assets[].
        const string bare = @"<head><HeadOutlet /></head><body><Routes /><script src=""_framework/blazor.web.js""></script></body>";

        var result = InitService.RewriteAppRazor(bare);

        Assert.Contains(@"<script src=""shellui.js""></script>", result);
        Assert.True(result.IndexOf(@"shellui.js") < result.IndexOf(@"blazor.web.js"));
    }

    [Fact]
    public void RewriteAppRazor_IsIdempotent()
    {
        var once = InitService.RewriteAppRazor(FreshAppRazor);
        var twice = InitService.RewriteAppRazor(once);

        Assert.Equal(once, twice);
    }

    [Theory]
    [InlineData("standalone")]
    [InlineData("npm")]
    public void TargetsFile_IsValidMsBuildXml(string method)
    {
        var targets = InitService.GetTargetsFileContent(method);

        var xml = System.Xml.Linq.XDocument.Parse(targets);
        Assert.Equal("Project", xml.Root!.Name.LocalName);
        if (method == "standalone")
        {
            Assert.Contains($"<TailwindTag>{ShellUI.Core.TailwindConstants.GitHubTag}</TailwindTag>", targets);
            Assert.Contains("<DownloadFile ", targets);
            Assert.Contains("WarnMissingTailwindCLI", targets);
        }
    }

    [Fact]
    public void RewriteAppRazor_KeepsIdentityPagesStatic()
    {
        var once = InitService.RewriteAppRazor(FreshAppRazor, identity: true);

        Assert.Contains(@"<HeadOutlet @rendermode=""PageRenderMode"" />", once);
        Assert.Contains(@"<Routes @rendermode=""PageRenderMode"" />", once);
        Assert.Contains(@"StartsWithSegments(""/Account"") ? null : InteractiveServer", once);
        Assert.Equal(once, InitService.RewriteAppRazor(once, identity: true));
    }

    [Fact]
    public void RewriteAppRazor_PreservesExistingRenderMode()
    {
        const string custom =
            @"<HeadOutlet @rendermode=""InteractiveAuto"" />" + "\n" +
            @"<Routes @rendermode=""InteractiveAuto"" />";

        var result = InitService.RewriteAppRazor(custom);

        Assert.Contains(@"<HeadOutlet @rendermode=""InteractiveAuto"" />", result);
        Assert.Contains(@"<Routes @rendermode=""InteractiveAuto"" />", result);
        Assert.DoesNotContain(@"InteractiveServer", result);
    }

    [Fact]
    public void RewriteWasmIndexHtml_InjectsThemeAndShelluiJs()
    {
        const string indexHtml = @"<!DOCTYPE html>
<html>
<head>
    <title>App</title>
</head>
<body>
    <div id=""app""></div>
    <script src=""_framework/blazor.webassembly.js""></script>
</body>
</html>";

        var result = InitService.RewriteWasmIndexHtml(indexHtml);

        Assert.Contains("ShellUI theme bootstrap", result);
        var shelluiIdx = result.IndexOf(@"<script src=""shellui.js""></script>");
        var blazorIdx = result.IndexOf(@"<script src=""_framework/blazor.webassembly.js""");
        Assert.True(shelluiIdx > 0 && shelluiIdx < blazorIdx);
    }

    [Fact]
    public void RewriteWasmIndexHtml_IsIdempotent()
    {
        const string indexHtml = @"<!DOCTYPE html><html><head></head><body><script src=""_framework/blazor.webassembly.js""></script></body></html>";

        var once = InitService.RewriteWasmIndexHtml(indexHtml);
        var twice = InitService.RewriteWasmIndexHtml(once);

        Assert.Equal(once, twice);
    }
}

public class RequiredImportsTests
{
    [Fact]
    public void VariantsAndModelsFiles_AddTheirNamespaces()
    {
        var imports = ComponentInstaller.RequiredImports("App", Contents("button", "button-variants", "command-models")).ToList();

        Assert.Equal(new[] { "@using App.Components.UI.Variants", "@using App.Components.Models" }, imports);
    }

    [Fact]
    public void PlainComponents_AddNothing()
    {
        Assert.Empty(ComponentInstaller.RequiredImports("App", Contents("kbd", "shellui-js")));
    }

    [Fact]
    public void VariantsFileInTheUiNamespace_AddsNothing()
    {
        // AvatarVariants sits in Variants/ but declares Components.UI; a Variants using would not compile.
        Assert.Empty(ComponentInstaller.RequiredImports("App", Contents("avatar", "avatar-variants")));
    }

    private static IEnumerable<string> Contents(params string[] names) =>
        names.Select(n => ComponentRegistry.GetComponentContent(n) ?? throw new InvalidOperationException(n));
}

public class InputCssTests
{
    [Fact]
    public void BaseLayer_HidesFocusOutlineOnNavigatedHeading()
    {
        // FocusOnNavigate focuses the page h1; the stock app.css rule that hides its outline is overwritten by init.
        Assert.Contains("h1:focus {\n    outline: none;", ShellUI.Templates.CssTemplates.InputCss.Replace("\r\n", "\n"));
    }
}

public class RootNamespaceTests
{
    // Matches what the compiler and the .NET 10 template produce for the same project names.
    [Theory]
    [InlineData("my-app", "my_app")]
    [InlineData("AllApp-net9.0", "AllApp_net9._0")]
    [InlineData("Contoso.Shop", "Contoso.Shop")]
    [InlineData("1app", "_1app")]
    [InlineData("My App", "My_App")]
    public void SanitizeNamespace_MakesAValidNamespace(string value, string expected)
    {
        Assert.Equal(expected, ProjectDetector.SanitizeNamespace(value));
    }
}
