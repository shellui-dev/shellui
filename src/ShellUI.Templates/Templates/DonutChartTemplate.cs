using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DonutChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "donut-chart",
        DisplayName = "Donut Chart",
        Description = "Donut chart component using ShellCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "DonutChart.razor",
        Dependencies = new List<string> { "chart" },
        Tags = new List<string> { "chart", "donut", "data", "visualization", "shellcharts" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ShellCharts
@inherits Chart<TItem>
@typeparam TItem

<ChartFrame Title=""@Title"" Subtitle=""@Subtitle"" Class=""@Class"" Width=""@Width"" Theme=""Theme"">
    <ChartContainer Data=""Data"" Height=""PlotHeight"" AriaLabel=""@(Title ?? Name)"">
        <Donut TItem=""TItem"" DataKey=""Value"" NameKey=""Category"" Name=""@Name"" PaddingAngle=""2"" />
        <CenterLabel Value=""@ChartFormat.Number(Data?.Sum(d => Value(d) ?? 0) ?? 0)"" Label=""@Name"" />
        <ChartTooltip />
        @if (ShowLegend)
        {
            <ChartLegend />
        }
    </ChartContainer>
</ChartFrame>
";
}
