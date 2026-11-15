using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-description",
        DisplayName = "Sheet Description",
        Description = "Description subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetDescription.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "description" }
    };

    public static string Content => ComponentSource.Read("Components/SheetDescription.razor");
}
