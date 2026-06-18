using System.Linq;
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
