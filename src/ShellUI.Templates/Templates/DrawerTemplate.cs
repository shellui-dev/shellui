using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer",
        DisplayName = "Drawer",
        Description = "Sliding drawer component with handle — supports compositional subcomponent pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "Drawer.razor",
        Dependencies = new List<string> { "drawer-variants", "drawer-trigger", "drawer-content", "drawer-header", "drawer-title", "drawer-description", "drawer-footer", "drawer-close" },
        Tags = new List<string> { "overlay", "drawer", "modal", "slide" }
    };

    public static string Content => ComponentSource.Read("Components/Drawer.razor");
}


