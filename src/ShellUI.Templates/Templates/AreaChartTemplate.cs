using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AreaChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "area-chart",
        DisplayName = "Area Chart",
        Description = "Area chart component using ApexCharts with ShellUI theming",
        Category = ComponentCategory.DataDisplay,
        FilePath = "AreaChart.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "area", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/AreaChart.razor");
}