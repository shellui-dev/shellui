namespace ShellUI.Templates.Templates;

public static class AlertDialogTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog",
        Description = "Modal dialog for confirmations and alerts",
        Category = ComponentCategory.Overlay,
        Dependencies = new[] { "dialog", "button" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI

<Dialog IsOpen=""IsOpen"" Title=""@Title"" Description=""@Description"" OnClose=""OnBackdropClick"">
    <Footer>
        @if (CancelText != null)
        {
            <button class=""mt-3 inline-flex w-full justify-center rounded-md border border-input bg-background px-4 py-2 text-sm font-medium shadow-sm hover:bg-accent hover:text-accent-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 sm:mt-0 sm:w-auto"" @onclick=""HandleCancel"">@CancelText</button>
        }
        <button class=""@($""inline-flex w-full justify-center rounded-md px-4 py-2 text-sm font-medium shadow-sm focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 sm:w-auto {(ConfirmVariant == ""destructive"" ? ""bg-destructive text-destructive-foreground hover:bg-destructive/90"" : ConfirmVariant == ""outline"" ? ""border border-input bg-background hover:bg-accent hover:text-accent-foreground"" : ConfirmVariant == ""secondary"" ? ""bg-secondary text-secondary-foreground hover:bg-secondary/80"" : ConfirmVariant == ""ghost"" ? ""hover:bg-accent hover:text-accent-foreground"" : ConfirmVariant == ""link"" ? ""text-primary underline-offset-4 hover:underline"" : ""bg-primary text-primary-foreground hover:bg-primary/90"")}"")"" @onclick=""HandleConfirm"">@ConfirmText</button>
    </Footer>
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
    public string ConfirmVariant { get; set; } = ""default"";

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

    private async Task OnBackdropClick()
    {
        await CloseDialog();
    }

    private async Task CloseDialog()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(IsOpen);
    }
}";
}
