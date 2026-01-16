using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class EmptyStateTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "empty-state",
        DisplayName = "Empty State",
        Description = "An empty state component for when there's no data to display",
        Category = ComponentCategory.Feedback,
        FilePath = "EmptyState.razor",
        Tags = new List<string> { "empty", "state", "placeholder", "no-data" },
        Dependencies = new List<string> { "button" }
    };

    public const string Content = """
<div class="flex flex-col items-center justify-center py-12 text-center">
    @if (!string.IsNullOrEmpty(Icon))
    {
        <div class="mb-4">
            @((MarkupString)Icon)
        </div>
    }
    
    @if (!string.IsNullOrEmpty(Title))
    {
        <h3 class="mb-2 text-lg font-semibold text-foreground">@Title</h3>
    }
    
    @if (!string.IsNullOrEmpty(Description))
    {
        <p class="mb-6 max-w-sm text-sm text-muted-foreground">@Description</p>
    }
    
    @if (Action != null)
    {
        <div class="flex flex-col gap-2 sm:flex-row">
            @Action
        </div>
    }
    
    @if (ChildContent != null)
    {
        <div class="mt-4">
            @ChildContent
        </div>
    }
</div>

@code {
    [Parameter] public string? Icon { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public RenderFragment? Action { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;
}
""";
}

