using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class HoverCardContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "hover-card-content",
        DisplayName = "Hover Card Content",
        Description = "Content panel for HoverCard (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "HoverCardContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "hover-card", "content" }
    };

    public static string Content => ComponentSource.Read("Components/HoverCardContent.razor");
}
