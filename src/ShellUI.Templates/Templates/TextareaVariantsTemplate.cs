using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TextareaVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "textarea-variants",
        DisplayName = "Textarea Variants",
        Description = "Textarea utility classes (uses InputVariant)",
        Category = ComponentCategory.Utility,
        FilePath = "Variants/TextareaVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string> { "input-variants" }
    };

    public static string Content => ComponentSource.Read("Variants/TextareaVariants.cs", variantsNamespace: true);
}
