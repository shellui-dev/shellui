using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ChartVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chart-variants",
        DisplayName = "Chart Variants",
        Description = "Chart theming and styling utilities for ShellUI charts",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Variants/ChartVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Variants = new List<string> { "default", "colorful", "monochrome" },
        Tags = new List<string> { "chart", "theme", "styling", "data", "visualization" }
    };

    public static string Content => ComponentSource.Read("Variants/ChartVariants.cs", variantsNamespace: true);
}
