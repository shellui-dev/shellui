using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AccordionTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion-trigger",
        DisplayName = "Accordion Trigger",
        Description = "Trigger button for AccordionItem (shadcn-style)",
        Category = ComponentCategory.Layout,
        FilePath = "AccordionTrigger.razor",
        Dependencies = new List<string> { "accordion-item" },
        IsAvailable = false,
        Tags = new List<string> { "layout", "accordion", "trigger", "toggle" }
    };

    public static string Content => ComponentSource.Read("Components/AccordionTrigger.razor");
}
