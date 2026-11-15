using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BreadcrumbPageTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "breadcrumb-page",
        DisplayName = "Breadcrumb Page",
        Description = "Current-page (unlinked) subcomponent for the compositional Breadcrumb pattern",
        Category = ComponentCategory.Layout,
        FilePath = "BreadcrumbPage.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "breadcrumb", "page" }
    };

    public static string Content => ComponentSource.Read("Components/BreadcrumbPage.razor");
}
