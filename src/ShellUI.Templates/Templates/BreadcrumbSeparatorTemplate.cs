using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BreadcrumbSeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "breadcrumb-separator",
        DisplayName = "Breadcrumb Separator",
        Description = "Separator (default chevron) for the compositional Breadcrumb pattern",
        Category = ComponentCategory.Layout,
        FilePath = "BreadcrumbSeparator.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "breadcrumb", "separator" }
    };

    public static string Content => ComponentSource.Read("Components/BreadcrumbSeparator.razor");
}
