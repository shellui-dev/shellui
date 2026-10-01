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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<span role=""link"" aria-disabled=""true"" aria-current=""page"" class=""@Shell.Cn(""font-normal text-foreground"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</span>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
