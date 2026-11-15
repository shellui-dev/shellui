using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ToggleVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "toggle-variants",
        DisplayName = "Toggle Variants",
        Description = "Variant definitions for Toggle component",
        Category = ComponentCategory.Form,
        FilePath = "Variants/ToggleVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Variants/ToggleVariants.cs");
}


