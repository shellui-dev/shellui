using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AccordionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion",
        DisplayName = "Accordion",
        Description = "Collapsible content sections",
        Category = ComponentCategory.Layout,
        FilePath = "Accordion.razor",

        Tags = new List<string> { "layout", "collapsible", "accordion" },
        Dependencies = new List<string> { "accordion-type", "accordion-item", "accordion-trigger", "accordion-content" }
    };

    public static string Content => ComponentSource.Read("Components/Accordion.razor");
}


