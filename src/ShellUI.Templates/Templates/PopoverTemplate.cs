using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class PopoverTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "popover",
        DisplayName = "Popover",
        Description = "Popover content component",
        Category = ComponentCategory.Overlay,
        FilePath = "Popover.razor",
        Version = "0.1.0",
        Tags = new List<string> { "popover", "overlay", "popup", "dropdown" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""relative inline-block @ClassName"" @attributes=""AdditionalAttributes"">
    <div @onclick=""Toggle"">
        @Trigger
    </div>
    
    @if (IsOpen)
    {
        <div class=""@(""absolute z-50 w-72 rounded-md border border-border bg-popover p-4 text-popover-foreground shadow-md outline-none animate-in fade-in-0 zoom-in-95 "" + (Placement == ""top"" ? ""bottom-full left-0 mb-2"" : Placement == ""left"" ? ""right-full top-0 mr-2"" : Placement == ""right"" ? ""left-full top-0 ml-2"" : ""top-full left-0 mt-2""))"">
            @ChildContent
        </div>
    }
</div>

@if (IsOpen)
{
    <div class=""fixed inset-0 z-40"" @onclick=""Close""></div>
}

@code {
    [Parameter]
    public bool IsOpen { get; set; }
    
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }
    
    [Parameter]
    public RenderFragment? Trigger { get; set; }
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string Placement { get; set; } = ""bottom"";
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private async Task Toggle()
    {
        IsOpen = !IsOpen;
        await IsOpenChanged.InvokeAsync(IsOpen);
    }
    
    private async Task Close()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(IsOpen);
    }
}
";
}

