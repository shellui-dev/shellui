using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-footer",
        DisplayName = "Sheet Footer",
        Description = "Footer subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetFooter.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "footer" }
    };

    public static string Content => ComponentSource.Read("Components/SheetFooter.razor");
}
