using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-footer",
        DisplayName = "Card Footer",
        Description = "Footer for the Card component",
        Category = ComponentCategory.Layout,

        FilePath = "CardFooter.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/CardFooter.razor");
}


