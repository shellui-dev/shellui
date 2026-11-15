using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-description",
        DisplayName = "Drawer Description",
        Description = "Description subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerDescription.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "description" }
    };

    public static string Content => ComponentSource.Read("Components/DrawerDescription.razor");
}
