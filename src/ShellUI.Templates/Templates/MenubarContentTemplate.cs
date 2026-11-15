using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-content",
        DisplayName = "Menubar Content",
        Description = "Popover content for the compositional Menubar pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menubar", "content" }
    };

    public static string Content => ComponentSource.Read("Components/MenubarContent.razor");
}
