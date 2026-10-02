using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationNextTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-next",
        DisplayName = "Pagination Next",
        Description = "Next-page button for the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationNext.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "next" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<PaginationLink Href=""@Href"" Size=""default"" Class=""@Shell.Cn(""gap-1 pr-2.5"", Class)"" OnClick=""OnClick"" aria-label=""Go to next page"" @attributes=""AdditionalAttributes"">
    <span>Next</span>
    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M9 5l7 7-7 7"" />
    </svg>
</PaginationLink>

@code {
    [Parameter] public string? Href { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
