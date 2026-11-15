using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-title",
        DisplayName = "Drawer Title",
        Description = "Title subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerTitle.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "title" }
    };

    public static string Content => ComponentSource.Read("Components/DrawerTitle.razor");
}
