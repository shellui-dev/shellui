using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog",
        DisplayName = "Dialog",
        Description = "Modal dialog component with composable parts",
        Category = ComponentCategory.Overlay,
        Version = "0.1.0",
        FilePath = "Dialog.razor",
        Dependencies = new List<string> { "dialog-trigger", "dialog-content", "dialog-header", "dialog-footer", "dialog-title", "dialog-description", "dialog-close" }
    };

    public static string Content => @"@namespace ShellUI.Components

<CascadingValue Value=""this"" IsFixed=""true"">
    @ChildContent
</CascadingValue>

@code {
    [Parameter] public bool Open { get; set; }
    [Parameter] public EventCallback<bool> OpenChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public async Task SetOpen(bool value)
    {
        if (Open != value)
        {
            Open = value;
            await OpenChanged.InvokeAsync(value);
            StateHasChanged();
        }
    }
}
";
}
