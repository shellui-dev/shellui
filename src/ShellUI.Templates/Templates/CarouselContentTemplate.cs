using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselContentTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "CarouselContent",
        Description = "Content wrapper for carousel items",
        Category = ComponentCategory.Display,
        Tags = new[] { "carousel", "content", "wrapper" }
    };

    public const string Content = """
<div class="@($"relative flex w-full {Class}")">
    @ChildContent
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;
}
""";
}
