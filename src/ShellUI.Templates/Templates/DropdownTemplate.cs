using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DropdownTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dropdown",
        DisplayName = "Dropdown",
        Description = "Dropdown menu with trigger and content",
        Category = ComponentCategory.Overlay,

        FilePath = "Dropdown.razor",
        Dependencies = new List<string> { "dropdown-trigger", "dropdown-content", "dropdown-item" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<CascadingValue Value=""this"" IsFixed=""true"">
    <div class=""@Shell.Cn(""relative inline-block text-left"", ClassName, Class)"" @attributes=""AdditionalAttributes"">
        @if (UseCompositional)
        {
            @ChildContent
        }
        else
        {
            <button type=""button""
                    @onclick=""ToggleOpen""
                    class=""inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 border border-input bg-background hover:bg-accent hover:text-accent-foreground h-10 pl-4 pr-3 py-2"">
                @Trigger
            </button>
            @if (IsOpen)
            {
                <div class=""absolute right-0 z-50 mt-2 w-56 origin-top-right rounded-md border border-border bg-popover p-1 text-popover-foreground shadow-md animate-in fade-in-0 zoom-in-95"">
                    @ChildContent
                </div>
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
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool UseCompositional => Trigger is null;

    public async Task ToggleAsync()
    {
        IsOpen = !IsOpen;
        await IsOpenChanged.InvokeAsync(IsOpen);
        StateHasChanged();
    }

    public async Task CloseAsync()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(IsOpen);
        StateHasChanged();
    }

    private async Task ToggleOpen()
    {
        await ToggleAsync();
    }
}
";
}


