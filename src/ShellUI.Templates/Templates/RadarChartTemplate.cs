using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class RadarChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "radar-chart",
        DisplayName = "Radar Chart",
        Description = "Radar chart component using ShellCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "RadarChart.razor",
        Dependencies = new List<string> { "chart" },
        Tags = new List<string> { "chart", "radar", "data", "visualization", "shellcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ShellCharts
@inherits Chart<TItem>
@typeparam TItem

<ChartFrame Title=""@Title"" Subtitle=""@Subtitle"" Class=""@Class"" Width=""@Width"" Theme=""Theme"">
    <ChartContainer Data=""Data"" Height=""PlotHeight"" AriaLabel=""@(Title ?? Name)"">
        <PolarGrid />
        <PolarAngleAxis TItem=""TItem"" DataKey=""Category"" />
        <Radar TItem=""TItem"" DataKey=""Value"" Name=""@Name"" />
        <ChartTooltip />
        @if (ShowLegend)
        {
            <ChartLegend />
        }
    </ChartContainer>
</ChartFrame>
";
}
