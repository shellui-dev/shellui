using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationEllipsisTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-ellipsis",
        DisplayName = "Pagination Ellipsis",
        Description = "Ellipsis placeholder between page numbers in the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationEllipsis.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "ellipsis" }
    };

    public static string Content => ComponentSource.Read("Components/PaginationEllipsis.razor");
}
