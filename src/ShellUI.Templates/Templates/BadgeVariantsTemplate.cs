using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class BadgeVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "badge-variants",
        DisplayName = "Badge Variants",
        Description = "Variant definitions for Badge component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Variants/BadgeVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Variants/BadgeVariants.cs");
}


