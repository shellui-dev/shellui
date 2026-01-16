using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-trigger",
        DisplayName = "Dialog Trigger",
        Description = "Trigger button for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogTrigger.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Toggle"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] public Dialog? Dialog { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Toggle()
    {
        if (Dialog != null)
        {
            await Dialog.SetOpen(!Dialog.Open);
        }
    }
}
";
}


