using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarMenuTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-menu",
        DisplayName = "Menubar Menu",
        Description = "Submenu container for the compositional Menubar pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarMenu.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menubar", "menu" }
    };

    public static string Content => ComponentSource.Read("Components/MenubarMenu.razor");
}
