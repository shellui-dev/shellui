using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselPreviousTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "carousel-previous",
        DisplayName = "Carousel Previous",
        Description = "Previous button for carousel navigation",
        Category = ComponentCategory.Navigation,
        FilePath = "CarouselPrevious.razor",
        IsAvailable = false,
        Tags = new List<string> { "carousel", "button", "previous", "navigation" }
    };

    public static string Content => ComponentSource.Read("Components/CarouselPrevious.razor");
}

