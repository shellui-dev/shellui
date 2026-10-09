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
        Dependencies = new List<string> { "popover-trigger", "popover-content" },

        Tags = new List<string> { "popover", "overlay", "popup", "dropdown" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<CascadingValue Value=""this"" IsFixed=""true"">
    <div class=""@Shell.Cn(""relative inline-block"", ClassName, Class)"" @attributes=""AdditionalAttributes"" @onkeydown=""OnKeyDown"">
        @if (Trigger is null)
        {
            @ChildContent
        }
        else
        {
            <div @onclick=""Toggle"" role=""button"" tabindex=""0"">@Trigger</div>
            @if (IsOpen)
            {
                <div class=""@Shell.Cn(""absolute z-50 w-72 rounded-md border border-border bg-popover p-4 text-popover-foreground shadow-md outline-none animate-in fade-in-0 zoom-in-95"", PlacementClass)"">@ChildContent</div>
            }
        }
    </div>
    @if (IsOpen)
    {
        <div class=""fixed inset-0 z-40"" @onclick=""CloseAsync""></div>
    }
</CascadingValue>

@code {
    [Parameter] public RenderFragment? Trigger { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
    [Parameter] public string Placement { get; set; } = ""bottom"";
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    internal string PlacementClass => Placement switch
    {
        ""top"" => ""bottom-full left-0 mb-2"",
        ""left"" => ""right-full top-0 mr-2"",
        ""right"" => ""left-full top-0 ml-2"",
        _ => ""top-full left-0 mt-2""
    };

    public async Task ToggleAsync() { IsOpen = !IsOpen; await IsOpenChanged.InvokeAsync(IsOpen); StateHasChanged(); }
    public async Task CloseAsync() { IsOpen = false; await IsOpenChanged.InvokeAsync(IsOpen); StateHasChanged(); }
    private async Task Toggle() => await ToggleAsync();
    private async Task Close() => await CloseAsync();

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == ""Escape"" && IsOpen) await CloseAsync();
    }
}
";
}


