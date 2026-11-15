using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarSeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-separator",
        DisplayName = "Sidebar Separator",
        Description = "Horizontal divider for sidebar sections",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarSeparator.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarSeparator.razor");
}
