using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart",
        DisplayName = "Chart",
        Description = "Base chart component with ShellUI theming and ApexCharts integration",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Chart.razor",
        Dependencies = new List<string> { "chart-variants", "chart-styles" },
        NuGetDependencies = new List<NuGetDependency>
        {
            new() { PackageId = "Blazor-ApexCharts", Version = "6.0.2" }
        },
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "data", "visualization", "apexcharts" }
    };

    public static string Content => ComponentSource.Read("Components/Chart.razor", variantsNamespace: true);
}