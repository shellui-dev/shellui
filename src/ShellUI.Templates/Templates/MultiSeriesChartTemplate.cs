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
        Tags = new List<string> { "chart", "multi-series", "data", "visualization", "shellcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ShellCharts
@inherits Chart<TItem>
@typeparam TItem
@attribute [CascadingTypeParameter(nameof(TItem))]

<ChartFrame Title=""@Title"" Subtitle=""@Subtitle"" Class=""@Class"" Width=""@Width"" Theme=""Theme"">
    <ChartContainer Data=""Data"" Height=""PlotHeight"" AriaLabel=""@Title"">
        <CartesianGrid />
        <XAxis TItem=""TItem"" TKey=""string"" DataKey=""Category"" />
        <YAxis />
        @ChildContent
        <ChartTooltip />
        @if (ShowLegend)
        {
            <ChartLegend />
        }
    </ChartContainer>
</ChartFrame>

@code {
    /// <summary>The <see cref=""ChartSeries{TItem}""/>, which all share this chart's <c>Data</c> and <c>XValue</c>.</summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
";
}