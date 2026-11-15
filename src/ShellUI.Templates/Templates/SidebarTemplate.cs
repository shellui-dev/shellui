using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar",
        DisplayName = "Sidebar",
        Description = "Dashboard sidebar with collapsible icon mode, mobile support, and inset variant (shadcn sidebar-08)",
        Category = ComponentCategory.Layout,

        FilePath = "Sidebar.razor",
        Dependencies = new List<string>
        {
            "shell", "sidebar-models", "sidebar-provider",
            "sidebar-header", "sidebar-content", "sidebar-footer",
            "sidebar-group", "sidebar-group-label", "sidebar-group-content",
            "sidebar-menu", "sidebar-menu-item", "sidebar-menu-button",
            "sidebar-menu-sub", "sidebar-menu-sub-item", "sidebar-menu-sub-button",
            "sidebar-menu-action", "sidebar-menu-badge",
            "sidebar-separator", "sidebar-trigger", "sidebar-inset", "sidebar-rail"
        },
        Tags = new List<string> { "sidebar", "navigation", "layout", "dashboard" }
    };

    public static string Content => ComponentSource.Read("Components/Sidebar.razor");
}


