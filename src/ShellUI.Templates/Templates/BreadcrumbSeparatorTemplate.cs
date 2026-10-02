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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<li role=""presentation"" aria-hidden=""true"" class=""@Shell.Cn(""[&>svg]:h-3.5 [&>svg]:w-3.5"", Class)"" @attributes=""AdditionalAttributes"">
    @if (ChildContent != null)
    {
        @ChildContent
    }
    else
    {
        <svg fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M9 5l7 7-7 7"" />
        </svg>
    }
</li>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
