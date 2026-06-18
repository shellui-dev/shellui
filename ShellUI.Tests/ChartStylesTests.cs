using ShellUI.CLI.Services;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class ChartStylesRegistryTests
{
    [Fact]
    public void ChartStyles_IsRegisteredAndHidden()
    {
        var metadata = ComponentRegistry.GetMetadata("chart-styles");
        Assert.NotNull(metadata);
        Assert.False(metadata!.IsAvailable, "chart-styles is a CSS asset bundled with chart, not a standalone install target");
    }

    [Fact]
    public void Chart_DependsOnChartStyles()
    {
        var metadata = ComponentRegistry.GetMetadata("chart");
        Assert.NotNull(metadata);
        Assert.Contains("chart-styles", metadata!.Dependencies);
    }

    [Fact]
    public void ChartStyles_FilePathTargetsWwwroot()
    {
        var metadata = ComponentRegistry.GetMetadata("chart-styles");
        Assert.NotNull(metadata);
        // The `../../wwwroot/` prefix walks out of Components/UI/ to project root,
        // matching the shellui-js convention. Without it the file lands inside the
        // component tree and is never served.
        Assert.StartsWith("../../wwwroot/", metadata!.FilePath);
        Assert.EndsWith(".css", metadata.FilePath);
    }

    [Fact]
    public void ChartStyles_ContentCoversCustomTooltipAndApexClasses()
    {
        var content = ComponentRegistry.GetComponentContent("chart-styles");
        Assert.NotNull(content);
        // Custom tooltip emitted by ChartVariants' Custom HTML — these were invisible
        // before this branch because the CSS template existed but was never installed.
        Assert.Contains(".custom-tooltip", content);
        Assert.Contains(".custom-tooltip-title", content);
        Assert.Contains(".custom-tooltip-item", content);
        Assert.Contains(".custom-tooltip-marker", content);
        Assert.Contains(".custom-tooltip-label", content);
        Assert.Contains(".custom-tooltip-value", content);
        // ApexCharts built-in classes also styled so charts without the custom HTML still look right.
        Assert.Contains(".apexcharts-tooltip", content);
        Assert.Contains(".apexcharts-legend", content);
        Assert.Contains(".apexcharts-xaxis-label", content);
        // Theme-aware: values come from the CSS variables the init script ships.
        Assert.Contains("var(--popover", content);
        Assert.Contains("var(--foreground", content);
        Assert.Contains("var(--border", content);
    }
}

public class StylesheetInjectionTests
{
    [Fact]
    public void ResolveHostStylesheetHref_StripsWwwrootPrefix()
    {
        Assert.Equal("css/charts.css", ComponentInstaller.ResolveHostStylesheetHref("../../wwwroot/css/charts.css"));
    }

    [Fact]
    public void ResolveHostStylesheetHref_IgnoresNonWwwrootFilePaths()
    {
        Assert.Null(ComponentInstaller.ResolveHostStylesheetHref("Button.razor"));
        Assert.Null(ComponentInstaller.ResolveHostStylesheetHref("Variants/ButtonVariants.cs"));
    }

    [Fact]
    public void ResolveHostStylesheetHref_IgnoresNonCssAssets()
    {
        // shellui.js also uses ../../wwwroot/ but is a script, not a stylesheet.
        Assert.Null(ComponentInstaller.ResolveHostStylesheetHref("../../wwwroot/shellui.js"));
    }

    [Fact]
    public void InjectStylesheetLink_AddsLinkBeforeHeadClose()
    {
        const string app = "<html><head><HeadOutlet /></head><body></body></html>";

        var result = InitService.InjectStylesheetLink(app, "css/charts.css");

        Assert.Contains("<link href=\"css/charts.css\" rel=\"stylesheet\" />", result);
        var linkIdx = result.IndexOf("<link href=\"css/charts.css\"");
        var headCloseIdx = result.IndexOf("</head>");
        Assert.True(linkIdx > 0 && linkIdx < headCloseIdx, "link tag must sit inside <head>");
    }

    [Fact]
    public void InjectStylesheetLink_IsIdempotent()
    {
        const string app = "<html><head><HeadOutlet /></head><body></body></html>";

        var once = InitService.InjectStylesheetLink(app, "css/charts.css");
        var twice = InitService.InjectStylesheetLink(once, "css/charts.css");

        Assert.Equal(once, twice);
    }

    [Fact]
    public void InjectStylesheetLink_SupportsMultipleDifferentHrefs()
    {
        const string app = "<html><head><HeadOutlet /></head><body></body></html>";

        var result = InitService.InjectStylesheetLink(app, "css/charts.css");
        result = InitService.InjectStylesheetLink(result, "css/another.css");

        Assert.Contains("css/charts.css", result);
        Assert.Contains("css/another.css", result);
    }
}
