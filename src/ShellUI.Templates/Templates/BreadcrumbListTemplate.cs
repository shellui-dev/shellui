using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BreadcrumbListTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "breadcrumb-list",
        DisplayName = "Breadcrumb List",
        Description = "Ordered-list wrapper for the compositional Breadcrumb pattern",
        Category = ComponentCategory.Layout,
        FilePath = "BreadcrumbList.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "breadcrumb", "list" }
    };

    public static string Content => ComponentSource.Read("Components/BreadcrumbList.razor");
}
