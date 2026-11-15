using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuSubTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-sub",
        DisplayName = "Sidebar Menu Sub",
        Description = "Nested sub-menu container for sidebar items",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuSub.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarMenuSub.razor");
}
