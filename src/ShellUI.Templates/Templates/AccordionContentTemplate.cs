using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AccordionContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion-content",
        DisplayName = "Accordion Content",
        Description = "Expandable content for AccordionItem (shadcn-style)",
        Category = ComponentCategory.Layout,
        FilePath = "AccordionContent.razor",
        Dependencies = new List<string> { "accordion-item" },
        IsAvailable = false,
        Tags = new List<string> { "layout", "accordion", "content", "body" }
    };

    public static string Content => ComponentSource.Read("Components/AccordionContent.razor");
}
