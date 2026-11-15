using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class LinkCardTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "link-card",
        DisplayName = "Link Card",
        Description = "Card-style links for related pages",
        Category = ComponentCategory.Layout,
        FilePath = "LinkCard.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "docs", "card", "link", "navigation" }
    };

    public static string Content => ComponentSource.Read("Components/LinkCard.razor");
}
