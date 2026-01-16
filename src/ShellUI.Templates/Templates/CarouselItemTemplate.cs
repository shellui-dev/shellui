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
        Tags = new List<string> { "carousel", "item", "slide", "content" }
    };

    public const string Content = """
<div class="@($"w-full flex-shrink-0 {Class}")" style="width: @(100.0 / TotalSlides)%">
    <div class="w-full h-full flex items-center justify-center">
        @ChildContent
    </div>
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public int TotalSlides { get; set; } = 1;
}
""";
}

