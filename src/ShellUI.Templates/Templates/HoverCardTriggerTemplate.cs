using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class HoverCardTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "hover-card-trigger",
        DisplayName = "Hover Card Trigger",
        Description = "Hover trigger for HoverCard (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "HoverCardTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "hover-card", "trigger" }
    };

    public static string Content => ComponentSource.Read("Components/HoverCardTrigger.razor");
}
