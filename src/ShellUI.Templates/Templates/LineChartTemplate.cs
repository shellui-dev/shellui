using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class LineChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "line-chart",
        DisplayName = "Line Chart",
        Description = "Line chart component using ShellCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "LineChart.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "line", "data", "visualization", "shellcharts" }
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
        <Line TItem=""TItem"" DataKey=""Value"" Name=""@Name"" />
        <ChartTooltip />
        @if (ShowLegend)
        {
            <ChartLegend />
        }
    </ChartContainer>
</ChartFrame>
";
}