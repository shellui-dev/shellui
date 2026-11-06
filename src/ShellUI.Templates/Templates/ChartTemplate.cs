using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart",
        DisplayName = "Chart",
        Description = "Base for the ready-made charts, which draw with ShellCharts",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Chart.razor",
        Dependencies = new List<string> { "chart-variants", "chart-styles", "chart-frame" },
        NuGetDependencies = new List<NuGetDependency>
        {
            new() { PackageId = "ShellCharts.Blazor", Version = "0.1.0-alpha" }
        },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "data", "visualization", "shellcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants
@typeparam TItem

@* Parameters shared by the ready-made charts, which draw with ShellCharts.
   For anything they don't cover, compose ShellCharts parts inside a ChartFrame. *@

@code {
    [Parameter] public IEnumerable<TItem>? Data { get; set; }

    /// <summary>The series name, for the legend and tooltip.</summary>
    [Parameter] public string Name { get; set; } = ""Data"";

    /// <summary>Each item's category: its label on the x axis, or the name of its slice, ring or spoke.</summary>
    [Parameter] public Func<TItem, object>? XValue { get; set; }

    [Parameter] public Func<TItem, decimal?>? YValue { get; set; }

    [Parameter] public ChartTheme Theme { get; set; }

    [Parameter] public string? Title { get; set; }

    [Parameter] public string? Subtitle { get; set; }

    [Parameter] public string? Class { get; set; }

    /// <summary>The plot's height, such as ""300px"".</summary>
    [Parameter] public string? Height { get; set; } = ""300px"";

    [Parameter] public string? Width { get; set; } = ""100%"";

    [Parameter] public bool ShowLegend { get; set; } = true;

    [Parameter, Obsolete(""ShellCharts has no toolbar, so this does nothing."")]
    public bool ShowToolbar { get; set; }

    protected int? PlotHeight => int.TryParse(Height?.Replace(""px"", """").Trim(), out var px) ? px : null;

    protected string Category(TItem item) => XValue?.Invoke(item)?.ToString() ?? """";

    protected double? Value(TItem item) => (double?)YValue?.Invoke(item);
}
";
}