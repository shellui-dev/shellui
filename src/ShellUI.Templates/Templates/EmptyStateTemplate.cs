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

    public static string Content => ComponentSource.Read("Components/EmptyState.razor");
}

