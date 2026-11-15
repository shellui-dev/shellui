using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselContentTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "carousel-content",
        DisplayName = "Carousel Content",
        Description = "Content wrapper for carousel items",
        Category = ComponentCategory.DataDisplay,
        FilePath = "CarouselContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "carousel", "content", "wrapper" }
    };

    public static string Content => ComponentSource.Read("Components/CarouselContent.razor");
}

