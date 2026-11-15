using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DonutChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "donut-chart",
        DisplayName = "Donut Chart",
        Description = "Donut chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "DonutChart.razor",
        Dependencies = new List<string> { "chart" },
        Tags = new List<string> { "chart", "donut", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/DonutChart.razor");
}
