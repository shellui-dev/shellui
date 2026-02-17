using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselSlideTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "carousel-slide",
        DisplayName = "Carousel Slide",
        Description = "Individual slide for Carousel (value-based)",
        Category = ComponentCategory.DataDisplay,
        FilePath = "CarouselSlide.razor",
        Dependencies = new List<string> { "carousel" },
        IsAvailable = false,
        Tags = new List<string> { "carousel", "slideshow", "slide" }
    };

    public static string Content => "";
}
