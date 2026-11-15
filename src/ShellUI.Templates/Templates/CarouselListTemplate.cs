using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselListTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "carousel-list",
        DisplayName = "Carousel List",
        Description = "Sliding viewport for Carousel (value-based)",
        Category = ComponentCategory.DataDisplay,
        FilePath = "CarouselList.razor",
        IsAvailable = false,
        Tags = new List<string> { "carousel", "slideshow", "list" }
    };

    public static string Content => ComponentSource.Read("Components/CarouselList.razor");
}
