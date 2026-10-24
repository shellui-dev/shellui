using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class RadialChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "radial-chart",
        DisplayName = "Radial Chart",
        Description = "Radial bar chart component using ShellCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "RadialChart.razor",
        Dependencies = new List<string> { "chart" },
        Tags = new List<string> { "chart", "radial", "data", "visualization", "shellcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ShellCharts
@inherits Chart<TItem>
@typeparam TItem

@* Values are percentages: 100 fills a ring. *@
<ChartFrame Title=""@Title"" Subtitle=""@Subtitle"" Class=""@Class"" Width=""@Width"" Theme=""Theme"">
    <ChartContainer Data=""Data"" Height=""PlotHeight"" AriaLabel=""@(Title ?? Name)"">
        <RadialBar TItem=""TItem"" DataKey=""Value"" NameKey=""Category"" Name=""@Name"" Max=""100"" Background />
        <ChartTooltip />
        @if (ShowLegend)
        {
            <ChartLegend />
        }
    </ChartContainer>
</ChartFrame>
";
}
