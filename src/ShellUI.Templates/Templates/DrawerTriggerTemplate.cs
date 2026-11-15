using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-trigger",
        DisplayName = "Drawer Trigger",
        Description = "Trigger subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerTrigger.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "drawer", "trigger" }
    };

    public static string Content => ComponentSource.Read("Components/DrawerTrigger.razor");
}
