using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ToggleGroupItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "toggle-group-item",
        DisplayName = "Toggle Group Item",
        Description = "Item for ToggleGroup",
        Category = ComponentCategory.Form,
        FilePath = "ToggleGroupItem.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "toggle", "group" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@implements IDisposable

<button type=""button""
        role=""@(Group?.Multiple == true ? ""checkbox"" : ""radio"")""
        aria-checked=""@(IsPressed ? ""true"" : ""false"")""
        data-state=""@(IsPressed ? ""on"" : ""off"")""
        disabled=""@IsDisabled""
        @onclick=""ToggleAsync""
        class=""@Shell.Cn(""inline-flex h-9 min-w-9 items-center justify-center gap-2 rounded-md px-2.5 text-sm font-medium transition-colors hover:bg-muted hover:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 [&>svg]:size-4"", IsPressed ? ""bg-accent text-accent-foreground"" : ""bg-transparent"", Class)""
        @attributes=""AdditionalAttributes"">
    @ChildContent
</button>

@code {
    [CascadingParameter] public ToggleGroup? Group { get; set; }
    [Parameter] public string Value { get; set; } = """";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsPressed => Group?.IsPressed(Value) == true;
    private bool IsDisabled => Disabled || Group?.Disabled == true;

    protected override void OnInitialized() => Group?.Register(this);

    internal void Refresh() => InvokeAsync(StateHasChanged);

    private async Task ToggleAsync()
    {
        if (Group != null) await Group.ToggleAsync(Value);
    }

    public void Dispose() => Group?.Unregister(this);
}
";
}
