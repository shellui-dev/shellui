namespace ShellUI.Templates.Templates;

public static class AlertDialogTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog",
        Description = "Modal dialog for confirmations and alerts",
        Category = ComponentCategory.Overlay,
        Dependencies = new[] { "button" }
    };

    public static string Content => @"
@using Microsoft.JSInterop

<div class=""@($""relative z-50 {(IsOpen ? """" : ""hidden"")}"")"">
    <div class=""fixed inset-0 z-50 bg-background/80 backdrop-blur-sm"" @onclick=""OnBackdropClick""></div>
    <div class=""fixed left-[50%] top-[50%] z-50 grid w-full max-w-lg translate-x-[-50%] translate-y-[-50%] gap-4 border bg-background p-6 shadow-lg duration-200 sm:rounded-lg"">
        @if (!string.IsNullOrEmpty(Title))
        {
            <div class=""flex flex-col space-y-1.5 text-center sm:text-left"">
                <h2 class=""text-lg font-semibold leading-none tracking-tight"">@Title</h2>
                @if (!string.IsNullOrEmpty(Description))
                {
                    <p class=""text-sm text-muted-foreground"">@Description</p>
                }
            </div>
        }

        <div class=""flex flex-col-reverse sm:flex-row sm:justify-end sm:space-x-2"">
            @if (CancelText != null)
            {
                <Button Variant=""outline"" @onclick=""OnCancel"">@CancelText</Button>
            }
            <Button Variant=""@ConfirmVariant"" @onclick=""OnConfirm"">@ConfirmText</Button>
        </div>
    </div>
</div>

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

    private async Task OnConfirm()
    {
        await OnConfirm.InvokeAsync();
        await CloseDialog();
    }

    private async Task OnCancel()
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
