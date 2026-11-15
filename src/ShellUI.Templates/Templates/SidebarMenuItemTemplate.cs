using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-item",
        DisplayName = "Sidebar Menu Item",
        Description = "Individual menu item in the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuItem.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarMenuItem.razor");
}
