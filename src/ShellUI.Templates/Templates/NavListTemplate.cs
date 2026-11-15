using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavListTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "nav-list",
        DisplayName = "Nav List",
        Description = "List container for NavigationMenu items",
        Category = ComponentCategory.Navigation,
        FilePath = "NavList.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "nav", "list" }
    };

    public static string Content => ComponentSource.Read("Components/NavList.razor");
}
