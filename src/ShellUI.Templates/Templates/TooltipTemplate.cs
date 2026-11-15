using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TooltipTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tooltip",
        DisplayName = "Tooltip",
        Description = "Hover tooltip component",
        Category = ComponentCategory.Feedback,
        FilePath = "Tooltip.razor",

        Tags = new List<string> { "tooltip", "hover", "feedback", "popover" }
    };

    public static string Content => ComponentSource.Read("Components/Tooltip.razor");
}


