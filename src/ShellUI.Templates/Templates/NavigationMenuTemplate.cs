using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class NavigationMenuTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "navigation-menu",
        DisplayName = "Navigation Menu",
        Description = "Navigation menu component",
        Category = ComponentCategory.Navigation,
        FilePath = "NavigationMenu.razor",

        Tags = new List<string> { "navigation", "menu", "nav" },
        Dependencies = new List<string> { "navigation-menu-item", "nav-list", "nav-item", "nav-trigger", "nav-content" }
    };

    public static string Content => ComponentSource.Read("Components/NavigationMenu.razor");
}


