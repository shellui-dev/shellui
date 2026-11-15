using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination",
        DisplayName = "Pagination",
        Description = "Pagination component for navigating pages",
        Category = ComponentCategory.Navigation,
        FilePath = "Pagination.razor",
        Dependencies = new List<string> { "pagination-content", "pagination-item", "pagination-link", "pagination-previous", "pagination-next", "pagination-ellipsis" },

        Tags = new List<string> { "navigation", "pagination", "pages" }
    };

    public static string Content => ComponentSource.Read("Components/Pagination.razor");
}


