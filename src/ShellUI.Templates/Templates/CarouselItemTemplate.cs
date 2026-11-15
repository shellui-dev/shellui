using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselItemTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "carousel-item",
        DisplayName = "Carousel Item",
        Description = "An individual item within a carousel",
        Category = ComponentCategory.DataDisplay,
        FilePath = "CarouselItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "carousel", "item", "slide", "content" }
    };

    public static string Content => ComponentSource.Read("Components/CarouselItem.razor");
}

