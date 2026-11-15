using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ButtonVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "button-variants",
        DisplayName = "Button Variants",
        Description = "Button variant enums and utility classes",
        Category = ComponentCategory.Utility,

        FilePath = "Variants/ButtonVariants.cs",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Variants/ButtonVariants.cs", variantsNamespace: true);
}

