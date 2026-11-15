using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-item",
        DisplayName = "Menubar Item",
        Description = "Individual menubar item",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menu", "menubar", "item" }
    };

    public static string Content => ComponentSource.Read("Components/MenubarItem.razor");
}


