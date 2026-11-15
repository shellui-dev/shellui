using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartSeriesTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart-series",
        DisplayName = "Chart Series",
        Description = "Individual chart series component for use in multi-series charts",
        Category = ComponentCategory.DataDisplay,
        FilePath = "ChartSeries.razor",
        Dependencies = new List<string> { "chart" },
        Variants = new List<string> { "line", "bar", "area", "pie" },
        Tags = new List<string> { "chart", "series", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/ChartSeries.razor");
}