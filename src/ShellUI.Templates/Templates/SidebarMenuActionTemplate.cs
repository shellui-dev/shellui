using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuActionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-action",
        DisplayName = "Sidebar Menu Action",
        Description = "Action button overlay for sidebar menu items",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuAction.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarMenuAction.razor");
}
