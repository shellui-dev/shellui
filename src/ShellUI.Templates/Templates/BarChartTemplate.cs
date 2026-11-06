using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BarChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "bar-chart",
        DisplayName = "Bar Chart",
        Description = "Bar chart component using ShellCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "BarChart.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "bar", "data", "visualization", "shellcharts" }
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
        <Bar TItem=""TItem"" DataKey=""Value"" Name=""@Name"" />
        <ChartTooltip />
        @if (ShowLegend)
        {
            <ChartLegend />
        }
    </ChartContainer>
</ChartFrame>
";
}