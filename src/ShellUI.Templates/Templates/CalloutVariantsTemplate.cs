using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CalloutVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "callout-variants",
        DisplayName = "Callout Variants",
        Description = "Variant definitions for Callout component",
        Category = ComponentCategory.Feedback,
        FilePath = "Variants/CalloutVariants.cs",
        Dependencies = new List<string>(),
        IsAvailable = false,
        Tags = new List<string> { "callout", "variants", "docs", "admonition" }
    };

    public static string Content => ComponentSource.Read("Variants/CalloutVariants.cs", variantsNamespace: true);
}
