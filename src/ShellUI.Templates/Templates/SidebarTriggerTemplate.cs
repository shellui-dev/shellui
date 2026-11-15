using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-trigger",
        DisplayName = "Sidebar Trigger",
        Description = "Toggle button to open/close the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarTrigger.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarTrigger.razor");
}
