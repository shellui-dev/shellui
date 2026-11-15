using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ScrollAreaTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "scroll-area",
        DisplayName = "Scroll Area",
        Description = "Custom scrollable container component",
        Category = ComponentCategory.Layout,
        FilePath = "ScrollArea.razor",

        Tags = new List<string> { "layout", "scroll", "overflow", "container" }
    };

    public static string Content => ComponentSource.Read("Components/ScrollArea.razor");
}


