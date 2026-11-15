using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CalloutTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "callout",
        DisplayName = "Callout",
        Description = "Info/warning/danger/tip callout boxes for documentation",
        Category = ComponentCategory.Feedback,
        FilePath = "Callout.razor",
        Dependencies = new List<string> { "callout-variants" },
        Variants = new List<string> { "info", "warning", "danger", "tip", "default" },
        Tags = new List<string> { "docs", "callout", "admonition", "info", "warning", "tip" }
    };

    public static string Content => ComponentSource.Read("Components/Callout.razor", variantsNamespace: true);
}
