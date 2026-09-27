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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@{
    var count = Math.Max(1, Parent?.EffectiveSlideCount ?? 1);
    var idx = Parent?.CurrentIndex ?? 0;
    var pct = 100.0 / count;
}
<div class=""relative overflow-hidden rounded-lg"" style=""aspect-ratio: @(Parent?.AspectRatio ?? ""16/9"");"">
    <div class=""flex transition-transform duration-300 ease-in-out h-full""
         style=""transform: translateX(-@(idx * pct)%); width: @(count * 100)%"">
        @ChildContent
    </div>
</div>

@code {
    [CascadingParameter] public Carousel? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
";
}
