using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class HoverCardTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "hover-card",
        DisplayName = "Hover Card",
        Description = "Rich hover content component",
        Category = ComponentCategory.Overlay,
        FilePath = "HoverCard.razor",
        Dependencies = new List<string> { "hover-card-trigger", "hover-card-content" }
    };

    public static string Content => ComponentSource.Read("Components/HoverCard.razor");
}

