using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-content",
        DisplayName = "Drawer Content",
        Description = "Content subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerContent.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "drawer", "content" }
    };

    public static string Content => ComponentSource.Read("Components/DrawerContent.razor", variantsNamespace: true);
}
