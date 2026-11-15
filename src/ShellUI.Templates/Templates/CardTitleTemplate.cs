using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-title",
        DisplayName = "Card Title",
        Description = "Title for the Card component",
        Category = ComponentCategory.Layout,

        FilePath = "CardTitle.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/CardTitle.razor");
}


