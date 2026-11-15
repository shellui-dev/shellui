using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "carousel",
        DisplayName = "Carousel",
        Description = "A carousel component for displaying content in a slideshow format",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Carousel.razor",
        Tags = new List<string> { "carousel", "slideshow", "slider", "images", "content" },
        Dependencies = new List<string> { "carousel-item", "carousel-content", "carousel-previous", "carousel-next", "carousel-dots", "carousel-list", "carousel-slide" }
    };

    public static string Content => ComponentSource.Read("Components/Carousel.razor");
}

