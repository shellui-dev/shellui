using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-content",
        DisplayName = "Sheet Content",
        Description = "Content subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetContent.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "sheet", "content" }
    };

    public static string Content => ComponentSource.Read("Components/SheetContent.razor", variantsNamespace: true);
}
