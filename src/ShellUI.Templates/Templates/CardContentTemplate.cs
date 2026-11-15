using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-content",
        DisplayName = "Card Content",
        Description = "Content area for the Card component",
        Category = ComponentCategory.Layout,

        FilePath = "CardContent.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/CardContent.razor");
}


