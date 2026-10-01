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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<nav aria-label=""breadcrumb"" class=""@Class"" @attributes=""AdditionalAttributes"">
    @if (UseList)
    {
        @ChildContent
    }
    else
    {
        <ol class=""@Shell.Cn(""flex flex-wrap items-center gap-1.5 break-words text-sm text-muted-foreground sm:gap-2.5"", Class)"">
            @ChildContent
        </ol>
    }
</nav>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    // Set when the children include a BreadcrumbList, which renders the <ol> itself.
    [Parameter] public bool UseList { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}


