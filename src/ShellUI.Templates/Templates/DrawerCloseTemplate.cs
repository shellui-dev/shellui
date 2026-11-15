using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerCloseTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-close",
        DisplayName = "Drawer Close",
        Description = "Close button subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerClose.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "close" }
    };

    public static string Content => ComponentSource.Read("Components/DrawerClose.razor");
}
