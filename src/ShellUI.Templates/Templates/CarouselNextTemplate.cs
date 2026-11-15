using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselNextTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "carousel-next",
        DisplayName = "Carousel Next",
        Description = "Next button for carousel navigation",
        Category = ComponentCategory.Navigation,
        FilePath = "CarouselNext.razor",
        IsAvailable = false,
        Tags = new List<string> { "carousel", "button", "next", "navigation" }
    };

    public static string Content => ComponentSource.Read("Components/CarouselNext.razor");
}

