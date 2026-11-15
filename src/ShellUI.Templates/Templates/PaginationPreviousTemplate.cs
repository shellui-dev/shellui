using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationPreviousTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-previous",
        DisplayName = "Pagination Previous",
        Description = "Previous-page button for the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationPrevious.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "previous" }
    };

    public static string Content => ComponentSource.Read("Components/PaginationPrevious.razor");
}
