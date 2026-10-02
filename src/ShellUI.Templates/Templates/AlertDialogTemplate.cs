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
        Dependencies = new List<string> { "dialog", "button", "alert-dialog-trigger", "alert-dialog-content", "alert-dialog-header", "alert-dialog-title", "alert-dialog-description", "alert-dialog-footer", "alert-dialog-action", "alert-dialog-cancel" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants

@if (UseLegacy)
{
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
}
else
{
    <CascadingValue Value=""this"" IsFixed=""true"">
        @ChildContent
    </CascadingValue>
}

@code {
    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public string ConfirmText { get; set; } = ""Continue"";
    [Parameter] public string? CancelText { get; set; } = ""Cancel"";
    [Parameter] public ButtonVariant ConfirmVariant { get; set; } = ButtonVariant.Default;
    [Parameter] public EventCallback OnConfirm { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    private bool UseLegacy => !string.IsNullOrEmpty(Title) || !string.IsNullOrEmpty(Description);

    public async Task SetOpen(bool value)
    {
        if (IsOpen != value)
        {
            IsOpen = value;
            await IsOpenChanged.InvokeAsync(value);
            StateHasChanged();
        }
    }

    public async Task InvokeConfirm()
    {
        await OnConfirm.InvokeAsync();
        await SetOpen(false);
    }

    public async Task InvokeCancel()
    {
        await OnCancel.InvokeAsync();
        await SetOpen(false);
    }

    private async Task HandleConfirm() => await InvokeConfirm();
    private async Task HandleCancel() => await InvokeCancel();
}
";
}

