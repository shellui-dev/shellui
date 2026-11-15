using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-header",
        DisplayName = "Drawer Header",
        Description = "Header subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerHeader.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "header" }
    };

    public static string Content => ComponentSource.Read("Components/DrawerHeader.razor");
}
