using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-group",
        DisplayName = "Command Group",
        Description = "Group of Command options with a heading",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandGroup.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "group" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@implements IDisposable

@* Children stay rendered while hidden so their options keep their registration. *@
<CascadingValue Value=""this"" IsFixed=""true"">
    <div role=""group"" hidden=""@(!IsVisible)"" class=""@Shell.Cn(""overflow-hidden p-1 text-foreground"", Class)"" @attributes=""AdditionalAttributes"">
        @if (!string.IsNullOrEmpty(Heading))
        {
            <div class=""px-2 py-1.5 text-xs font-medium text-muted-foreground"">@Heading</div>
        }
        @ChildContent
    </div>
</CascadingValue>

@code {
    [CascadingParameter] private Command? Parent { get; set; }
    [Parameter] public string? Heading { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private readonly List<CommandOption> _options = new();

    private bool IsVisible => Parent == null || _options.Count == 0 || _options.Any(Parent.Matches);

    internal void Register(CommandOption option) => _options.Add(option);
    internal void Unregister(CommandOption option) => _options.Remove(option);

    protected override void OnInitialized()
    {
        if (Parent != null) Parent.Changed += OnChanged;
    }

    private void OnChanged() => InvokeAsync(StateHasChanged);

    public void Dispose()
    {
        if (Parent != null) Parent.Changed -= OnChanged;
    }
}
";
}
