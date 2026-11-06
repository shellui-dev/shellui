using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartSeriesTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart-series",
        DisplayName = "Chart Series",
        Description = "Individual chart series component for use in multi-series charts",
        Category = ComponentCategory.DataDisplay,
        FilePath = "ChartSeries.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "line", "bar", "area", "pie" },
        Tags = new List<string> { "chart", "series", "data", "visualization", "shellcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants
@using ShellCharts
@typeparam TItem

@switch (SeriesType)
{
    case SeriesType.Area:
        <Area TItem=""TItem"" DataKey=""Value"" Name=""@Name"" Stack=""@Stack"" />
        break;
    case SeriesType.Bar:
        <Bar TItem=""TItem"" DataKey=""Value"" Name=""@Name"" Stack=""@Stack"" />
        break;
    default:
        <Line TItem=""TItem"" DataKey=""Value"" Name=""@Name"" Stack=""@Stack"" />
        break;
}

@code {
    [Parameter] public string Name { get; set; } = ""Series"";

    [Parameter] public SeriesType SeriesType { get; set; } = SeriesType.Line;

    [Parameter] public Func<TItem, decimal?>? YValue { get; set; }

    /// <summary>Series with the same stack key are drawn on top of each other.</summary>
    [Parameter] public string? Stack { get; set; }

    private double? Value(TItem item) => (double?)YValue?.Invoke(item);
}
";
}