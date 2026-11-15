using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationLinkTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-link",
        DisplayName = "Pagination Link",
        Description = "Page-number link/button for the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationLink.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "link" }
    };

    public static string Content => ComponentSource.Read("Components/PaginationLink.razor");
}
