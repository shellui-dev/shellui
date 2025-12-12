using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AlertDialogTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog",
        DisplayName = "Alert Dialog",
        Description = "Modal dialog for confirmations and alerts",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialog.razor",
        Dependencies = new List<string> { "dialog", "button" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI

<Dialog Open=""IsOpen"" OpenChanged=""IsOpenChanged"">
    <DialogContent>
        <DialogHeader>
            <DialogTitle>@Title</DialogTitle>
            <DialogDescription>@Description</DialogDescription>
        </DialogHeader>
        <DialogFooter>
            @if (CancelText != null)
            {
                <Button Variant=""@ButtonVariant.Outline"" @onclick=""HandleCancel"">@CancelText</Button>
            }
            <Button Variant=""@ConfirmVariant"" @onclick=""HandleConfirm"">@ConfirmText</Button>
        </DialogFooter>
    </DialogContent>
</Dialog>

@code {
    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public string Title { get; set; } = """";

    [Parameter]
    public string Description { get; set; } = """";

    [Parameter]
    public string ConfirmText { get; set; } = ""Continue"";

    [Parameter]
    public string? CancelText { get; set; } = ""Cancel"";

    [Parameter]
    public ButtonVariant ConfirmVariant { get; set; } = ButtonVariant.Default;

    [Parameter]
    public EventCallback OnConfirm { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    private async Task HandleConfirm()
    {
        await OnConfirm.InvokeAsync();
        await CloseDialog();
    }

    private async Task HandleCancel()
    {
        await OnCancel.InvokeAsync();
        await CloseDialog();
    }

    private async Task CloseDialog()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(IsOpen);
    }
}";
}
