using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetCloseTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-close",
        DisplayName = "Sheet Close",
        Description = "Close button subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetClose.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "close" }
    };

    public static string Content => ComponentSource.Read("Components/SheetClose.razor");
}
