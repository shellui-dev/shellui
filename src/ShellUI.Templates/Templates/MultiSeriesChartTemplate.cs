using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MultiSeriesChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "multi-series-chart",
        DisplayName = "Multi Series Chart",
        Description = "Multi-series chart component for combining multiple data series",
        Category = ComponentCategory.DataDisplay,
        FilePath = "MultiSeriesChart.razor",
        Dependencies = new List<string> { "chart", "chart-series" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "multi-series", "data", "visualization", "apexcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ApexCharts
@inherits Chart<TItem>
@typeparam TItem where TItem : class

<ApexChart TItem=""TItem""
           Title=""@Title""
           Options=""@ChartOptions""
           Height=""@Height""
           Width=""@Width"">
    @ChildContent
</ApexChart>

@code {
    // ChildContent is inherited from Chart<TItem>
}
";
}