using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-variants",
        DisplayName = "Sheet Variants",
        Description = "SheetSide enum and SheetVariants utility class for Sheet subcomponents",
        Category = ComponentCategory.Utility,
        FilePath = "Variants/SheetVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "sheet", "variants" }
    };

    public static string Content => ComponentSource.Read("Variants/SheetVariants.cs", variantsNamespace: true);
}
