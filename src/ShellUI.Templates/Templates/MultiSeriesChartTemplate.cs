using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MultiSeriesChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "multi-series-chart",
        DisplayName = "Multi Series Chart",
        Description = "Multi-series chart component for combining multiple data series",
        Category = ComponentCategory.DataDisplay,
        FilePath = "MultiSeriesChart.razor",
        Dependencies = new List<string> { "chart", "chart-series" },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "multi-series", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/MultiSeriesChart.razor");
}