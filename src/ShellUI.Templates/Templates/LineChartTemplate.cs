using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class LineChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "line-chart",
        DisplayName = "Line Chart",
        Description = "Line chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "LineChart.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "line", "data", "visualization", "apexcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ApexCharts
@inherits Chart<TItem>
@typeparam TItem where TItem : class

<div class=""@ComputedClass border border-border bg-card text-card-foreground overflow-hidden [border-radius:var(--radius)] [box-shadow:var(--shadow)]"" data-chart-theme=""@Theme.ToString().ToLower()"">
    <ApexChart TItem=""TItem""
               Title=""@Title""
               Options=""@ChartOptions""
               Height=""@Height""
               Width=""@Width"">
        <ApexPointSeries TItem=""TItem""
                         Items=""Data""
                         Name=""@Name""
                         SeriesType=""SeriesType.Line""
                         XValue=""@XValue""
                         YValue=""@YValue"" />
    </ApexChart>
</div>

@code {
    [Parameter] public IEnumerable<TItem>? Data { get; set; }
    [Parameter] public string Name { get; set; } = ""Data"";
    [Parameter] public Func<TItem, object>? XValue { get; set; }
    [Parameter] public Func<TItem, decimal?>? YValue { get; set; }
}
";
}