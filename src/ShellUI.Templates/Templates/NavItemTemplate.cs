using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "nav-item",
        DisplayName = "Nav Item",
        Description = "Navigation item with optional dropdown",
        Category = ComponentCategory.Navigation,
        FilePath = "NavItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "nav", "item" }
    };

    public static string Content => ComponentSource.Read("Components/NavItem.razor");
}
