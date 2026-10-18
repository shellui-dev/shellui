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

<div class=""@Shell.Cn(""animate-pulse rounded-md bg-muted"", Variant == ""circle"" ? ""rounded-full"" : """", Variant == ""text"" ? ""h-4 w-full"" : """", ClassName, Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    // ""default"", ""circle"" or ""text"".
    [Parameter] public string Variant { get; set; } = ""default"";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}


