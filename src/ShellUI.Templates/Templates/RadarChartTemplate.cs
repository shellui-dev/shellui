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

    public static string Content => ComponentSource.Read("Components/RadarChart.razor");
}
