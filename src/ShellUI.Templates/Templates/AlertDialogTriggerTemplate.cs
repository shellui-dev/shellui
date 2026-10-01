using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-trigger",
        DisplayName = "Alert Dialog Trigger",
        Description = "Trigger subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Open"" role=""button"" tabindex=""0"" @onkeydown=""HandleKeyDown"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] private AlertDialog? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Open()
    {
        if (Parent != null) await Parent.SetOpen(true);
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if ((e.Key == ""Enter"" || e.Key == "" "") && Parent != null) await Parent.SetOpen(true);
    }
}
";
}
