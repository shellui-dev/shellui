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
        Dependencies = new List<string> { "carousel" },
        IsAvailable = false,
        Tags = new List<string> { "carousel", "slideshow", "list" }
    };

    public static string Content => "";
}
