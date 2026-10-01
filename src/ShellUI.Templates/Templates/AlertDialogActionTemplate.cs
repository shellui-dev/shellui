using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogActionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-action",
        DisplayName = "Alert Dialog Action",
        Description = "Confirm action button for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogAction.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "action" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<Button Variant=""@Variant"" Class=""@Class"" @onclick=""HandleClick"" @attributes=""AdditionalAttributes"">
    @ChildContent
</Button>

@code {
    [CascadingParameter] private AlertDialog? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public ButtonVariant Variant { get; set; } = ButtonVariant.Default;
    [Parameter] public string? Class { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
        if (Parent != null)
        {
            await Parent.InvokeConfirm();
        }
    }
}
";
}
