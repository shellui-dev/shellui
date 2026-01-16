using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TooltipTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tooltip",
        DisplayName = "Tooltip",
        Description = "Hover tooltip component",
        Category = ComponentCategory.Feedback,
        FilePath = "Tooltip.razor",

        Tags = new List<string> { "tooltip", "hover", "feedback", "popover" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""relative inline-block @ClassName"" 
     @onmouseenter=""Show"" 
     @onmouseleave=""Hide""
     @attributes=""AdditionalAttributes"">
    @Trigger
    
    @if (_isVisible)
    {
        <div class=""@(""absolute z-50 overflow-hidden rounded-md border border-border bg-popover px-3 py-1.5 text-sm text-popover-foreground shadow-md animate-in fade-in-0 zoom-in-95 "" + (Placement == ""bottom"" ? ""top-full left-1/2 -translate-x-1/2 mt-2"" : Placement == ""left"" ? ""right-full top-1/2 -translate-y-1/2 mr-2"" : Placement == ""right"" ? ""left-full top-1/2 -translate-y-1/2 ml-2"" : ""bottom-full left-1/2 -translate-x-1/2 mb-2""))"">
            @Content
        </div>
    }
</div>

@code {
    [Parameter]
    public RenderFragment? Trigger { get; set; }
    
    [Parameter]
    public RenderFragment? Content { get; set; }
    
    [Parameter]
    public string Placement { get; set; } = ""top"";
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private bool _isVisible;
    
    private void Show() => _isVisible = true;
    private void Hide() => _isVisible = false;
}
";
}


