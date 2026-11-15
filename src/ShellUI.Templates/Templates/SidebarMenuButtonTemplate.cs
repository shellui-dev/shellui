using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuButtonTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-button",
        DisplayName = "Sidebar Menu Button",
        Description = "Interactive button/link for sidebar menu items with tooltip and icon mode support",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuButton.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarMenuButton.razor");
}
