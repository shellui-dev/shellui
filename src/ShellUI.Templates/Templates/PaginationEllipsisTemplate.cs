using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationEllipsisTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-ellipsis",
        DisplayName = "Pagination Ellipsis",
        Description = "Ellipsis placeholder between page numbers in the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationEllipsis.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "ellipsis" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<span aria-hidden=""true"" class=""@Shell.Cn(""flex h-9 w-9 items-center justify-center"", Class)"" @attributes=""AdditionalAttributes"">
    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <circle cx=""5"" cy=""12"" r=""1"" fill=""currentColor"" />
        <circle cx=""12"" cy=""12"" r=""1"" fill=""currentColor"" />
        <circle cx=""19"" cy=""12"" r=""1"" fill=""currentColor"" />
    </svg>
    <span class=""sr-only"">More pages</span>
</span>

@code {
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
