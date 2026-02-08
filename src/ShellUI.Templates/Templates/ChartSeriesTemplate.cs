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
        Dependencies = new List<string>(),
        Variants = new List<string> { "line", "bar", "area", "pie" },
        Tags = new List<string> { "chart", "series", "data", "visualization", "apexcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ApexCharts
@typeparam TItem where TItem : class

<ApexPointSeries TItem=""TItem""
                 Items=""Data""
                 Name=""@Name""
                 SeriesType=""@SeriesType""
                 XValue=""@XValue""
                 YValue=""@YValue"" />

@code {
    [Parameter] public IEnumerable<TItem>? Data { get; set; }
    [Parameter] public string Name { get; set; } = ""Series"";
    [Parameter] public SeriesType SeriesType { get; set; } = SeriesType.Line;
    [Parameter] public Func<TItem, object>? XValue { get; set; }
    [Parameter] public Func<TItem, decimal?>? YValue { get; set; }
}
";
}