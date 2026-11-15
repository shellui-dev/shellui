using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AccordionItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion-item",
        DisplayName = "Accordion Item",
        Description = "Individual collapsible section within an Accordion",
        Category = ComponentCategory.Layout,
        FilePath = "AccordionItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "layout", "collapsible", "item" }
    };

    public static string Content => ComponentSource.Read("Components/AccordionItem.razor");
}


