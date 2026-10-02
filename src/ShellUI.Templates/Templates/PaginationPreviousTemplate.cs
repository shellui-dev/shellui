using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PaginationPreviousTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "pagination-previous",
        DisplayName = "Pagination Previous",
        Description = "Previous-page button for the compositional Pagination pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "PaginationPrevious.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "pagination", "previous" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<PaginationLink Href=""@Href"" Size=""default"" Class=""@Shell.Cn(""gap-1 pl-2.5"", Class)"" OnClick=""OnClick"" aria-label=""Go to previous page"" @attributes=""AdditionalAttributes"">
    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M15 19l-7-7 7-7"" />
    </svg>
    <span>Previous</span>
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
