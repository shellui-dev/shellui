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
        Version = "0.1.0",
        Tags = new List<string> { "navigation", "breadcrumb", "layout" },
        Dependencies = new List<string> { "breadcrumb-item" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<nav aria-label=""breadcrumb"" class=""@ClassName"" @attributes=""AdditionalAttributes"">
    <ol class=""flex flex-wrap items-center gap-1.5 break-words text-sm text-muted-foreground sm:gap-2.5"">
        @ChildContent
    </ol>
</nav>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

