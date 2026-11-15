using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BreadcrumbEllipsisTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "breadcrumb-ellipsis",
        DisplayName = "Breadcrumb Ellipsis",
        Description = "Ellipsis placeholder for collapsed breadcrumb segments",
        Category = ComponentCategory.Layout,
        FilePath = "BreadcrumbEllipsis.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "breadcrumb", "ellipsis" }
    };

    public static string Content => ComponentSource.Read("Components/BreadcrumbEllipsis.razor");
}
