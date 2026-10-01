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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<CascadingValue Value=""this"" IsFixed=""true"">
    <ol class=""@Shell.Cn(""flex flex-wrap items-center gap-1.5 break-words text-sm text-muted-foreground sm:gap-2.5"", Class)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </ol>
</CascadingValue>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
