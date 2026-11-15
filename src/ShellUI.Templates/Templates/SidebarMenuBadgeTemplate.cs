using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuBadgeTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-badge",
        DisplayName = "Sidebar Menu Badge",
        Description = "Notification badge for sidebar menu items",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuBadge.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarMenuBadge.razor");
}
