using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarMenuSubButtonTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-menu-sub-button",
        DisplayName = "Sidebar Menu Sub Button",
        Description = "Interactive button/link for sidebar sub-menu items",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarMenuSubButton.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarMenuSubButton.razor");
}
