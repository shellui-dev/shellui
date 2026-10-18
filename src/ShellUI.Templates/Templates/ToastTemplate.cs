using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ToastTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "toast",
        DisplayName = "Toast",
        Description = "Toast notification component",
        Category = ComponentCategory.Feedback,
        FilePath = "Toast.razor",

        Variants = new List<string> { "default", "destructive", "success", "warning" },
        Tags = new List<string> { "notification", "toast", "feedback", "alert" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@implements IDisposable

@if (IsVisible)
{
    <div class=""@Shell.Cn(""fixed z-50 flex max-h-screen w-full flex-col-reverse gap-2 p-4 sm:flex-col md:max-w-[420px]"", GetPositionClass())"">
        <div class=""@Shell.Cn(""pointer-events-auto group relative flex w-full items-center justify-between space-x-4 overflow-hidden rounded-md border p-6 pr-8 shadow-lg transition-all motion-reduce:transition-none"", GetVariantClass(), ClassName, Class)""
             @attributes=""AdditionalAttributes"">
            <div class=""grid gap-1"">
                @if (!string.IsNullOrEmpty(Title))
                {
                    <div class=""text-sm font-semibold"">@Title</div>
                }
                @if (!string.IsNullOrEmpty(Description))
                {
                    <div class=""text-sm opacity-90"">@Description</div>
                }
                @ChildContent
            </div>
            <button type=""button"" @onclick=""Close""
                    class=""absolute right-2 top-2 rounded-md p-1 opacity-70 ring-offset-background transition-opacity hover:opacity-100 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"">
                <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                    <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M6 18L18 6M6 6l12 12"" />
                </svg>
                <span class=""sr-only"">Close</span>
            </button>
        </div>
    </div>
}

@code {
    [Parameter] public bool IsVisible { get; set; }
    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Description { get; set; }
    // ""default"", ""destructive"", ""success"" or ""warning"".
    [Parameter] public string Variant { get; set; } = ""default"";
    // ""top-left"", ""top-right"", ""bottom-left"" or ""bottom-right"".
    [Parameter] public string Position { get; set; } = ""bottom-right"";
    // Milliseconds before the toast hides itself; 0 keeps it open.
    [Parameter] public int Duration { get; set; } = 5000;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private CancellationTokenSource? _dismiss;
    private bool _wasVisible;

    protected override void OnAfterRender(bool firstRender)
    {
        if (IsVisible && !_wasVisible && Duration > 0)
        {
            _dismiss?.Cancel();
            _dismiss?.Dispose();
            _dismiss = new CancellationTokenSource();
            _ = AutoDismissAsync(_dismiss.Token);
        }
        else if (!IsVisible && _wasVisible)
        {
            _dismiss?.Cancel();
        }
        _wasVisible = IsVisible;
    }

    private async Task AutoDismissAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(Duration, token);
            await InvokeAsync(async () =>
            {
                if (!IsVisible) return;
                IsVisible = false;
                await IsVisibleChanged.InvokeAsync(IsVisible);
                StateHasChanged();
            });
        }
        catch (TaskCanceledException) { }
    }

    private async Task Close()
    {
        _dismiss?.Cancel();
        IsVisible = false;
        await IsVisibleChanged.InvokeAsync(IsVisible);
    }

    private string GetVariantClass() => Variant switch
    {
        ""destructive"" => ""border-destructive bg-destructive text-destructive-foreground"",
        ""success"" => ""border-green-500 bg-green-50 text-green-900 dark:bg-green-900/20 dark:text-green-100"",
        ""warning"" => ""border-yellow-500 bg-yellow-50 text-yellow-900 dark:bg-yellow-900/20 dark:text-yellow-100"",
        _ => ""border-border bg-background text-foreground""
    };

    private string GetPositionClass() => Position switch
    {
        ""top-left"" => ""top-0 left-0"",
        ""top-right"" => ""top-0 right-0"",
        ""bottom-left"" => ""bottom-0 left-0"",
        _ => ""bottom-0 right-0""
    };

    public void Dispose()
    {
        _dismiss?.Cancel();
        _dismiss?.Dispose();
    }
}
";
}


