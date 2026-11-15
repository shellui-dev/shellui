using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarGroupContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-group-content",
        DisplayName = "Sidebar Group Content",
        Description = "Content wrapper for a sidebar navigation group",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarGroupContent.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarGroupContent.razor");
}
