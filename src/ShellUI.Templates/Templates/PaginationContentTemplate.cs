using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-content",
        DisplayName = "Pagination Content",
        Description = "List wrapper (<ul>) for the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "content" }
    };

    public static string Content => ComponentSource.Read("Components/PaginationContent.razor");
}
