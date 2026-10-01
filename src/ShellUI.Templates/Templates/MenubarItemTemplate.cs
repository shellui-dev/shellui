using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-item",
        DisplayName = "Menubar Item",
        Description = "Individual menubar item",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menu", "menubar", "item" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (!string.IsNullOrEmpty(Title))
{
    <div class=""@Shell.Cn(""relative"", ClassName, Class)"" @attributes=""AdditionalAttributes"">
        <button type=""button""
                @onclick=""Toggle""
                class=""flex cursor-default select-none items-center rounded-sm px-3 py-1.5 text-sm font-medium outline-none hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground"">
            @Title
        </button>

        @if (IsOpen)
        {
            <div class=""absolute left-0 top-full z-50 mt-1 min-w-[8rem] overflow-hidden rounded-md border border-border bg-popover p-1 shadow-md animate-in fade-in-0 zoom-in-95"">
                @ChildContent
            </div>
        }
    </div>

    @if (IsOpen)
    {
        <div class=""fixed inset-0 z-40"" @onclick=""Close""></div>
    }
}
else
{
    <button type=""button""
            disabled=""@Disabled""
            @onclick=""Select""
            class=""@ItemClass""
            @attributes=""AdditionalAttributes"">
        @ChildContent
    </button>
}

@code {
    [CascadingParameter] private MenubarMenu? Menu { get; set; }
    // Set for the dropdown form; leave empty for a clickable item.
    [Parameter] public string Title { get; set; } = """";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsOpen { get; set; }

    private string ItemClass => Shell.Cn(
        Menu != null
            ? ""relative flex w-full cursor-default select-none items-center rounded-sm px-2 py-1.5 text-sm outline-none hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground""
            : ""inline-flex items-center rounded-sm px-3 py-1.5 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring"",
        ""disabled:pointer-events-none disabled:opacity-50"",
        ClassName, Class);

    private void Toggle() => IsOpen = !IsOpen;
    private void Close() => IsOpen = false;

    private async Task Select(MouseEventArgs e)
    {
        await OnClick.InvokeAsync(e);
        Menu?.Close();
    }
}
";
}


