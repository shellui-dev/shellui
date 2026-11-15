using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CollapsibleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "collapsible",
        DisplayName = "Collapsible",
        Description = "Collapsible content component with Trigger and CollapsibleContent",
        Category = ComponentCategory.Layout,
        FilePath = "Collapsible.razor",
        Dependencies = new List<string> { "collapsible-trigger", "collapsible-content" },
        Tags = new List<string> { "layout", "collapsible", "toggle", "expand" }
    };

    public static string Content => ComponentSource.Read("Components/Collapsible.razor");
}
