using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PopoverTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "popover",
        DisplayName = "Popover",
        Description = "Popover content component",
        Category = ComponentCategory.Overlay,
        FilePath = "Popover.razor",
        Dependencies = new List<string> { "popover-trigger", "popover-content" },

        Tags = new List<string> { "popover", "overlay", "popup", "dropdown" }
    };

    public static string Content => ComponentSource.Read("Components/Popover.razor");
}


