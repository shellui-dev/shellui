using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarRailTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-rail",
        DisplayName = "Sidebar Rail",
        Description = "Thin clickable rail for collapsing/expanding the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarRail.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarRail.razor");
}
