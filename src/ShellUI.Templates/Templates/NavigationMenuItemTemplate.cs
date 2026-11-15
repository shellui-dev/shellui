using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class NavigationMenuItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "navigation-menu-item",
        DisplayName = "Navigation Menu Item",
        Description = "Individual navigation menu item",
        Category = ComponentCategory.Navigation,
        FilePath = "NavigationMenuItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menu", "nav", "item" }
    };

    public static string Content => ComponentSource.Read("Components/NavigationMenuItem.razor");
}


