using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SkeletonTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "skeleton",
        DisplayName = "Skeleton",
        Description = "Loading placeholder with pulse animation",
        Category = ComponentCategory.Feedback,

        FilePath = "Skeleton.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""animate-pulse rounded-md bg-muted @(Variant == ""circle"" ? ""rounded-full"" : """") @(Variant == ""text"" ? ""h-4 w-full"" : """") @ClassName @Class"" @attributes=""AdditionalAttributes""></div>

@code {
    [Parameter]
    public string Variant { get; set; } = ""default"";

    [Parameter]
    public string? Class { get; set; }

    // Deprecated alias for Class; kept so markup written against older ShellUI versions still applies.
    [Parameter]
    public string ClassName { get; set; } = """";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}


