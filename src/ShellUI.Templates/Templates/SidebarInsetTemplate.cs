using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarInsetTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-inset",
        DisplayName = "Sidebar Inset",
        Description = "Main content wrapper that adjusts layout based on sidebar variant",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarInset.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarInset.razor");
}
