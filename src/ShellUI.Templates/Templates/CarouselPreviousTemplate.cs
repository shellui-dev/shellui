using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CarouselPreviousTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "CarouselPrevious",
        Description = "Previous button for carousel navigation",
        Category = ComponentCategory.Navigation,
        Tags = new[] { "carousel", "button", "previous", "navigation" }
    };

    public const string Content = """
<button class="@($"absolute left-2 top-1/2 -translate-y-1/2 rounded-full bg-background/80 p-2 shadow-md transition-colors hover:bg-background/90 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 {Class}")"
        disabled="@Disabled"
        @onclick="OnClick">
    <svg class="h-4 w-4" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 19l-7-7 7-7" />
    </svg>
</button>

@code {
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;
}
""";
}
