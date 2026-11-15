using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PieChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pie-chart",
        DisplayName = "Pie Chart",
        Description = "Pie chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "PieChart.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "pie", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/PieChart.razor");
}
