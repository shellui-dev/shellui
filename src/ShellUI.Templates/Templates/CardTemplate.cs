using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card",
        DisplayName = "Card",
        Description = "Container component for grouping related content",
        Category = ComponentCategory.Layout,

        FilePath = "Card.razor",
        Dependencies = new List<string> { "card-header", "card-title", "card-description", "card-content", "card-footer" }
    };

    public static string Content => ComponentSource.Read("Components/Card.razor");
}

