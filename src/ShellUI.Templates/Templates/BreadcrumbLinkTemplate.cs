using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BreadcrumbLinkTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "breadcrumb-link",
        DisplayName = "Breadcrumb Link",
        Description = "Anchor subcomponent for the compositional Breadcrumb pattern",
        Category = ComponentCategory.Layout,
        FilePath = "BreadcrumbLink.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "breadcrumb", "link" }
    };

    public static string Content => ComponentSource.Read("Components/BreadcrumbLink.razor");
}
