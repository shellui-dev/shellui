using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class InputVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "input-variants",
        DisplayName = "Input Variants",
        Description = "Input variant enum and utility classes",
        Category = ComponentCategory.Utility,
        FilePath = "Variants/InputVariants.cs",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Variants/InputVariants.cs", variantsNamespace: true);
}
