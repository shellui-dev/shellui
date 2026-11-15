using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-variants",
        DisplayName = "Drawer Variants",
        Description = "DrawerSide enum and DrawerVariants utility class for Drawer subcomponents",
        Category = ComponentCategory.Utility,
        FilePath = "Variants/DrawerVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "drawer", "variants" }
    };

    public static string Content => ComponentSource.Read("Variants/DrawerVariants.cs", variantsNamespace: true);
}
