using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu",
        DisplayName = "Sidebar Menu",
        Description = "Menu list container for sidebar navigation items",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenu.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarMenu.razor");
}
