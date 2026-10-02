using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-content",
        DisplayName = "Alert Dialog Content",
        Description = "Content subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using Microsoft.JSInterop
@implements IAsyncDisposable
@inject IJSRuntime JS

@if (Parent?.IsOpen == true)
{
    <div class=""fixed inset-0 z-50 bg-black/80 animate-in fade-in-0""></div>
    <div @ref=""_content"" tabindex=""-1"" role=""alertdialog"" aria-modal=""true"" @onkeydown=""OnKeyDownAsync"" class=""@Shell.Cn(""outline-none fixed left-[50%] top-[50%] z-50 grid w-full max-w-lg translate-x-[-50%] translate-y-[-50%] gap-4 border border-border bg-background p-6 shadow-lg duration-200 animate-in fade-in-0 zoom-in-95 sm:rounded-lg"", Class)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] private AlertDialog? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool _isLocked;
    private bool _focused;
    private ElementReference _content;

    // Escape dismisses an alert dialog as Cancel; the backdrop does not.
    private async Task OnKeyDownAsync(KeyboardEventArgs e)
    {
        if (e.Key == ""Escape"" && Parent != null) await Parent.InvokeCancel();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var isOpen = Parent?.IsOpen == true;
        if (isOpen && !_focused)
        {
            _focused = true;
            try { await _content.FocusAsync(); } catch (InvalidOperationException) { }
        }
        else if (!isOpen) _focused = false;

        var shouldBeLocked = Parent?.IsOpen == true;
        if (shouldBeLocked && !_isLocked)
        {
            try { await JS.InvokeVoidAsync(""ShellUI.lockBodyScroll""); _isLocked = true; } catch { }
        }
        else if (!shouldBeLocked && _isLocked)
        {
            try { await JS.InvokeVoidAsync(""ShellUI.unlockBodyScroll""); _isLocked = false; } catch { }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_isLocked)
        {
            try { await JS.InvokeVoidAsync(""ShellUI.unlockBodyScroll""); } catch { }
        }
    }
}
";
}
