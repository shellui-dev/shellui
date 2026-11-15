using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-trigger",
        DisplayName = "Sheet Trigger",
        Description = "Trigger subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetTrigger.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "sheet", "trigger" }
    };

    public static string Content => ComponentSource.Read("Components/SheetTrigger.razor");
}
