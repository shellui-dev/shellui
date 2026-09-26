using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class RadarChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "radar-chart",
        DisplayName = "Radar Chart",
        Description = "Radar chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "RadarChart.razor",
        Dependencies = new List<string> { "chart" },
        Tags = new List<string> { "chart", "radar", "data", "visualization", "apexcharts" }
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
                         SeriesType=""SeriesType.Radar""
                         XValue=""@XValue""
                         YValue=""@YValue"" />
    </ApexChart>
</div>

@code {
    [Parameter] public IEnumerable<TItem>? Data { get; set; }
    [Parameter] public string Name { get; set; } = ""Data"";
    [Parameter] public Func<TItem, object>? XValue { get; set; }
    [Parameter] public Func<TItem, decimal?>? YValue { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        // Radar tooltips only trigger per point, not in shared mode.
        if (ChartOptions.Tooltip != null)
        {
            ChartOptions.Tooltip.Shared = false;
            ChartOptions.Tooltip.Intersect = true;
        }
    }
}
";
}
