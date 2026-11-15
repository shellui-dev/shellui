using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-footer",
        DisplayName = "Sidebar Footer",
        Description = "Footer section of the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarFooter.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarFooter.razor");
}
