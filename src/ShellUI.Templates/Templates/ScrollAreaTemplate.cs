using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ScrollAreaTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "scroll-area",
        DisplayName = "Scroll Area",
        Description = "Custom scrollable container component",
        Category = ComponentCategory.Layout,
        FilePath = "ScrollArea.razor",

        Tags = new List<string> { "layout", "scroll", "overflow", "container" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""relative overflow-hidden"", ClassName, Class)"" @attributes=""AdditionalAttributes"">
    <div class=""h-full w-full overflow-auto"" style=""@ScrollStyle"">
        @ChildContent
    </div>
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    // Fixed height. When neither Height nor MaxHeight is set, the area grows with its content up to 400px.
    [Parameter] public string? Height { get; set; }
    [Parameter] public string? MaxHeight { get; set; }
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string ScrollStyle =>
        (Height is null ? """" : $""height: {Height}; "") + $""max-height: {MaxHeight ?? (Height is null ? ""400px"" : ""none"")};"";
}
";
}


