using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-header",
        DisplayName = "Card Header",
        Description = "Header for the Card component",
        Category = ComponentCategory.Layout,

        FilePath = "CardHeader.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/CardHeader.razor");
}


