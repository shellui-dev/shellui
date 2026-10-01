using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-content",
        DisplayName = "Pagination Content",
        Description = "List wrapper (<ul>) for the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<ul class=""@Shell.Cn(""flex flex-row items-center gap-1"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</ul>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
