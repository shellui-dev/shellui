using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationNextTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-next",
        DisplayName = "Pagination Next",
        Description = "Next-page button for the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationNext.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "next" }
    };

    public static string Content => ComponentSource.Read("Components/PaginationNext.razor");
}
