using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class RadialChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "radial-chart",
        DisplayName = "Radial Chart",
        Description = "Radial bar chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "RadialChart.razor",
        Dependencies = new List<string> { "chart" },
        Tags = new List<string> { "chart", "radial", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/RadialChart.razor");
}
