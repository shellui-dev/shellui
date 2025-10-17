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
        Version = "0.1.0",
        Tags = new List<string> { "layout", "scroll", "overflow", "container" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@(""relative overflow-hidden "" + ClassName)"" style=""max-height: @MaxHeight;"" @attributes=""AdditionalAttributes"">
    <div class=""h-full w-full overflow-auto"">
        @ChildContent
    </div>
</div>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string MaxHeight { get; set; } = ""400px"";
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}

