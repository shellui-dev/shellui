using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "nav-content",
        DisplayName = "Nav Content",
        Description = "Dropdown content for NavItem",
        Category = ComponentCategory.Navigation,
        FilePath = "NavContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "nav", "content" }
    };

    public static string Content => ComponentSource.Read("Components/NavContent.razor");
}
