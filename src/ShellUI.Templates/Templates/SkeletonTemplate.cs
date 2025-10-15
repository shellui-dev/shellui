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
        Version = "0.1.0",
        FilePath = "Skeleton.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""animate-pulse rounded-md bg-muted @(Variant == ""circle"" ? ""rounded-full"" : """") @(Variant == ""text"" ? ""h-4 w-full"" : """") @ClassName"" @attributes=""AdditionalAttributes""></div>

@code {
    [Parameter]
    public string Variant { get; set; } = ""default"";

    [Parameter]
    public string ClassName { get; set; } = """";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

