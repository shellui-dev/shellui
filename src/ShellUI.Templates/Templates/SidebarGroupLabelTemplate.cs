using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarGroupLabelTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-group-label",
        DisplayName = "Sidebar Group Label",
        Description = "Label for a sidebar navigation group",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarGroupLabel.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/SidebarGroupLabel.razor");
}
