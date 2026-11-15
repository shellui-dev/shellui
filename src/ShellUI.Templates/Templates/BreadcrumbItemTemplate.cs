using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BreadcrumbItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "breadcrumb-item",
        DisplayName = "Breadcrumb Item",
        Description = "Individual breadcrumb item",
        Category = ComponentCategory.Layout,
        FilePath = "BreadcrumbItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "breadcrumb", "item" }
    };

    public static string Content => ComponentSource.Read("Components/BreadcrumbItem.razor");
}


