using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-footer",
        DisplayName = "Drawer Footer",
        Description = "Footer subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerFooter.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "footer" }
    };

    public static string Content => ComponentSource.Read("Components/DrawerFooter.razor");
}
