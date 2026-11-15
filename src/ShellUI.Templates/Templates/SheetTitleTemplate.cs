using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-title",
        DisplayName = "Sheet Title",
        Description = "Title subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetTitle.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "title" }
    };

    public static string Content => ComponentSource.Read("Components/SheetTitle.razor");
}
