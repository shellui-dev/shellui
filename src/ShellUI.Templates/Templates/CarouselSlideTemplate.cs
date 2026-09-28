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
        IsAvailable = false,
        Tags = new List<string> { "carousel", "slideshow", "slide" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@implements IDisposable

@{
    var count = Math.Max(1, Parent?.EffectiveSlideCount ?? 1);
}
<div class=""@Shell.Cn(""w-full flex-shrink-0"", Class)"" style=""width: @(100.0 / count)%"">
    <div class=""w-full h-full flex items-center justify-center"">
        @ChildContent
    </div>
</div>

@code {
    [CascadingParameter] public Carousel? Parent { get; set; }
    [Parameter] public string Value { get; set; } = """";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    protected override void OnInitialized()
    {
        Parent?.RegisterSlide(this);
    }

    public void Dispose()
    {
        Parent?.UnregisterSlide(this);
    }
}
";
}
