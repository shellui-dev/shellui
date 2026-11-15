using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-group",
        DisplayName = "Sidebar Group",
        Description = "Group container for sidebar navigation sections",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarGroup.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarGroup.razor");
}
