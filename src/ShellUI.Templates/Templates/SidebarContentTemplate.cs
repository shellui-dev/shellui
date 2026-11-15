using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-content",
        DisplayName = "Sidebar Content",
        Description = "Scrollable content area of the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarContent.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarContent.razor");
}
