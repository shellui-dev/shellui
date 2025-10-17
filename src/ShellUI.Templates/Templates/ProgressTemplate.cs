using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ProgressTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "progress",
        DisplayName = "Progress",
        Description = "Progress bar indicator with customizable height",
        Category = ComponentCategory.Feedback,
        Version = "0.1.0",
        FilePath = "Progress.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""relative w-full overflow-hidden rounded-full bg-secondary @ClassName"" style=""height: @Height"">
    <div class=""h-full w-full flex-1 bg-primary transition-all"" style=""transform: translateX(-@(100 - Value)%)""></div>
</div>

@code {
    [Parameter]
    public int Value { get; set; } = 0;

    [Parameter]
    public string Height { get; set; } = ""0.5rem"";

    [Parameter]
    public string ClassName { get; set; } = """";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

