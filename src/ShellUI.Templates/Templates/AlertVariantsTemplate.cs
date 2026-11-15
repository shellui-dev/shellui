using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AlertVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-variants",
        DisplayName = "Alert Variants",
        Description = "Variant definitions for Alert component",
        Category = ComponentCategory.Feedback,
        FilePath = "Variants/AlertVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Variants/AlertVariants.cs");
}


