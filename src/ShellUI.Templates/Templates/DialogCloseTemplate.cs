using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogCloseTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-close",
        DisplayName = "Dialog Close",
        Description = "Close button for Dialog",
        Category = ComponentCategory.Overlay,
        Version = "0.1.0",
        FilePath = "DialogClose.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Close"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] public Dialog? Dialog { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Close()
    {
        if (Dialog != null)
        {
            await Dialog.SetOpen(false);
        }
    }
}
";
}

