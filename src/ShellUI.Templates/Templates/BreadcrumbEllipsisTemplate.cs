using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class BreadcrumbEllipsisTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "breadcrumb-ellipsis",
        DisplayName = "Breadcrumb Ellipsis",
        Description = "Ellipsis placeholder for collapsed breadcrumb segments",
        Category = ComponentCategory.Layout,
        FilePath = "BreadcrumbEllipsis.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "breadcrumb", "ellipsis" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<span role=""presentation"" aria-hidden=""true"" class=""@Shell.Cn(""flex h-9 w-9 items-center justify-center"", Class)"" @attributes=""AdditionalAttributes"">
    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <circle cx=""5"" cy=""12"" r=""1"" fill=""currentColor"" />
        <circle cx=""12"" cy=""12"" r=""1"" fill=""currentColor"" />
        <circle cx=""19"" cy=""12"" r=""1"" fill=""currentColor"" />
    </svg>
    <span class=""sr-only"">More</span>
</span>

@code {
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
