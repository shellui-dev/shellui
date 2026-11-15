using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet",
        DisplayName = "Sheet",
        Description = "Side panel/drawer component with multiple positions — supports compositional subcomponent pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "Sheet.razor",
        Dependencies = new List<string> { "sheet-variants", "sheet-trigger", "sheet-content", "sheet-header", "sheet-title", "sheet-description", "sheet-footer", "sheet-close" },
        Tags = new List<string> { "overlay", "sheet", "drawer", "panel", "side" }
    };

    public static string Content => ComponentSource.Read("Components/Sheet.razor");
}


