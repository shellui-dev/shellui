using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuSubItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-sub-item",
        DisplayName = "Sidebar Menu Sub Item",
        Description = "Individual item in a sidebar sub-menu",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuSubItem.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarMenuSubItem.razor");
}
