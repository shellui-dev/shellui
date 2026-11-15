using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-description",
        DisplayName = "Card Description",
        Description = "Description for the Card component",
        Category = ComponentCategory.Layout,

        FilePath = "CardDescription.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/CardDescription.razor");
}


