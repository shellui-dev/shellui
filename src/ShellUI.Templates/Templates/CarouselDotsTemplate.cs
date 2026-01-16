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
        Tags = new List<string> { "carousel", "dots", "indicators", "navigation" }
    };

    public const string Content = """
<div class="@($"absolute bottom-4 left-1/2 -translate-x-1/2 flex space-x-2 {Class}")">
    @for (int i = 0; i < TotalSlides; i++)
    {
        <button class="@($"h-2 w-2 rounded-full transition-colors {(i == CurrentSlide ? "bg-primary" : "bg-muted")}")"
                @onclick="() => OnDotClick.InvokeAsync(i)">
        </button>
    }
</div>

@code {
    [Parameter] public int TotalSlides { get; set; }
    [Parameter] public int CurrentSlide { get; set; }
    [Parameter] public EventCallback<int> OnDotClick { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;
}
""";
}

