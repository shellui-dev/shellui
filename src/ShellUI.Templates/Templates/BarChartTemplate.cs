using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BarChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "bar-chart",
        DisplayName = "Bar Chart",
        Description = "Bar chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "BarChart.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "bar", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/BarChart.razor");
}