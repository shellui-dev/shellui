using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselDotsTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "carousel-dots",
        DisplayName = "Carousel Dots",
        Description = "Dot indicators for carousel navigation",
        Category = ComponentCategory.Navigation,
        FilePath = "CarouselDots.razor",
        IsAvailable = false,
        Tags = new List<string> { "carousel", "dots", "indicators", "navigation" }
    };

    public static string Content => ComponentSource.Read("Components/CarouselDots.razor");
}

