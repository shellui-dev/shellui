using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class LineChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "line-chart",
        DisplayName = "Line Chart",
        Description = "Line chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "LineChart.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "line", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/LineChart.razor");
}