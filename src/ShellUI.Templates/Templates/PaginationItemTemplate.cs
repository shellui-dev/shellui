using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-item",
        DisplayName = "Pagination Item",
        Description = "List item (<li>) wrapper for the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "item" }
    };

    public static string Content => ComponentSource.Read("Components/PaginationItem.razor");
}
