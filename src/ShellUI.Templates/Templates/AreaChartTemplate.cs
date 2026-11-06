using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AreaChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "area-chart",
        DisplayName = "Area Chart",
        Description = "Area chart component using ShellCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "AreaChart.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "area", "data", "visualization", "shellcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ShellCharts
@inherits Chart<TItem>
@typeparam TItem

<ChartFrame Title=""@Title"" Subtitle=""@Subtitle"" Class=""@Class"" Width=""@Width"" Theme=""Theme"">
    <ChartContainer Data=""Data"" Height=""PlotHeight"" AriaLabel=""@(Title ?? Name)"">
        <CartesianGrid />
        <XAxis TItem=""TItem"" TKey=""string"" DataKey=""Category"" />
        <YAxis />
        <Area TItem=""TItem"" DataKey=""Value"" Name=""@Name"" Gradient />
        <ChartTooltip />
        @if (ShowLegend)
        {
            <ChartLegend />
        }
    </ChartContainer>
</ChartFrame>
";
}