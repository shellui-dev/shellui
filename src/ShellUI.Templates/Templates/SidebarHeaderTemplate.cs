using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-header",
        DisplayName = "Sidebar Header",
        Description = "Header section of the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarHeader.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarHeader.razor");
}
