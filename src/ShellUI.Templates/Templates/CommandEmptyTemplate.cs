using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandEmptyTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-empty",
        DisplayName = "Command Empty",
        Description = "Shown when no Command option matches the search",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandEmpty.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "empty" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@implements IDisposable

@if (Parent != null && Parent.VisibleCount == 0)
{
    <div class=""@Shell.Cn(""py-6 text-center text-sm text-muted-foreground"", Class)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] private Command? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

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
