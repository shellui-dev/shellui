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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<a href=""@Href"" class=""@Shell.Cn(""transition-colors hover:text-foreground"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</a>

@code {
    [Parameter] public string? Href { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
