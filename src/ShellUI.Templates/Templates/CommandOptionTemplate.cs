using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandOptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-option",
        DisplayName = "Command Option",
        Description = "Selectable option in a compositional Command",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandOption.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "option" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@implements IDisposable

@* CommandOption rather than CommandItem: CommandItem is the model used by Command's Commands list. *@
@if (IsVisible)
{
    <div role=""option""
         aria-selected=""@(IsActive ? ""true"" : ""false"")""
         aria-disabled=""@(Disabled ? ""true"" : ""false"")""
         @onclick=""SelectAsync""
         @onmousemove=""Activate""
         class=""@Shell.Cn(""relative flex cursor-pointer select-none items-center gap-2 rounded-sm px-2 py-1.5 text-sm outline-none"", IsActive ? ""bg-accent text-accent-foreground"" : """", Disabled ? ""pointer-events-none opacity-50"" : """", Class)""
         @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] private Command? Parent { get; set; }
    [CascadingParameter] private CommandGroup? Group { get; set; }
    // The text matched against the search; the option is hidden while searching if it is empty.
    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback OnSelect { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    internal string Text => Value ?? """";

    private bool IsVisible => Parent == null || Parent.Matches(this);
    private bool IsActive => Parent?.ActiveOption == this;

    protected override void OnInitialized()
    {
        Group?.Register(this);
        if (Parent == null) return;
        Parent.Register(this);
        Parent.Changed += OnChanged;
    }

    private void OnChanged() => InvokeAsync(StateHasChanged);

    private void Activate()
    {
        if (!Disabled) Parent?.SetActive(this);
    }

    internal async Task SelectAsync()
    {
        if (!Disabled) await OnSelect.InvokeAsync();
    }

    public void Dispose()
    {
        Group?.Unregister(this);
        if (Parent == null) return;
        Parent.Changed -= OnChanged;
        Parent.Unregister(this);
    }
}
";
}
