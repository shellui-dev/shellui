using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BreadcrumbTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "breadcrumb",
        DisplayName = "Breadcrumb",
        Description = "Navigation breadcrumb trail",
        Category = ComponentCategory.Layout,
        FilePath = "Breadcrumb.razor",

        Tags = new List<string> { "navigation", "breadcrumb", "layout" },
        Dependencies = new List<string> { "breadcrumb-item", "breadcrumb-list", "breadcrumb-link", "breadcrumb-page", "breadcrumb-separator", "breadcrumb-ellipsis" }
    };

    public static string Content => ComponentSource.Read("Components/Breadcrumb.razor");
}


