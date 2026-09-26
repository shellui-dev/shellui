using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class RadialChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "radial-chart",
        DisplayName = "Radial Chart",
        Description = "Radial bar chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "RadialChart.razor",
        Dependencies = new List<string> { "chart" },
        Tags = new List<string> { "chart", "radial", "data", "visualization", "apexcharts" }
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
                         SeriesType=""SeriesType.RadialBar""
                         XValue=""@XValue""
                         YValue=""@YValue"" />
    </ApexChart>
</div>

@code {
    [Parameter] public IEnumerable<TItem>? Data { get; set; }
    [Parameter] public string Name { get; set; } = ""Progress"";
    [Parameter] public Func<TItem, object>? XValue { get; set; }
    [Parameter] public Func<TItem, decimal?>? YValue { get; set; }
}
";
}
