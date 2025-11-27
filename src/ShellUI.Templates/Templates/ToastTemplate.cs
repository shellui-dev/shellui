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
        Version = "0.1.0",
        Variants = new List<string> { "default", "destructive", "success", "warning" },
        Tags = new List<string> { "notification", "toast", "feedback", "alert" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@implements IAsyncDisposable

@if (IsVisible)
{
    <div class=""@(""fixed z-50 flex max-h-screen w-full flex-col-reverse gap-2 p-4 sm:flex-col md:max-w-[420px] "" + (Position == ""top-left"" ? ""top-0 left-0"" : Position == ""top-right"" ? ""top-0 right-0"" : Position == ""bottom-left"" ? ""bottom-0 left-0"" : ""bottom-0 right-0""))"">
        <div class=""@(""pointer-events-auto group relative flex w-full items-center justify-between space-x-4 overflow-hidden rounded-md border p-6 pr-8 shadow-lg transition-all text-foreground "" + (Variant == ""destructive"" ? ""border-red-500 bg-red-50 dark:bg-red-900/20 dark:border-red-500/50"" : Variant == ""success"" ? ""border-green-500 bg-green-50 dark:bg-green-900/20 dark:border-green-500/50"" : Variant == ""warning"" ? ""border-yellow-500 bg-yellow-50 dark:bg-yellow-900/20 dark:border-yellow-500/50"" : ""border-border bg-background"") + "" "" + ClassName)""
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
            <button @onclick=""Close""
                    class=""absolute right-2 top-2 rounded-md p-1 opacity-70 ring-offset-background transition-opacity hover:opacity-100 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"">
                <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                    <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M6 18L18 6M6 6l12 12"" />
                </svg>
            </button>
        </div>
    </div>
}

@code {
    [Parameter]
    public bool IsVisible { get; set; }
    
    [Parameter]
    public EventCallback<bool> IsVisibleChanged { get; set; }
    
    [Parameter]
    public string? Title { get; set; }
    
    [Parameter]
    public string? Description { get; set; }
    
    [Parameter]
    public string Variant { get; set; } = ""default"";
    
    [Parameter]
    public string Position { get; set; } = ""bottom-right"";
    
    [Parameter]
    public int Duration { get; set; } = 5000;
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private CancellationTokenSource? _cancellationTokenSource;
    private bool _previousVisible;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsVisible && !_previousVisible && Duration > 0)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            _ = StartAutoDismiss(_cancellationTokenSource.Token);
        }
        else if (!IsVisible && _previousVisible)
        {
            _cancellationTokenSource?.Cancel();
        }
        
        _previousVisible = IsVisible;
    }

    private async Task StartAutoDismiss(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(Duration, cancellationToken);
            if (!cancellationToken.IsCancellationRequested && IsVisible)
            {
                await InvokeAsync(async () =>
                {
                    IsVisible = false;
                    await IsVisibleChanged.InvokeAsync(IsVisible);
                    StateHasChanged();
                });
            }
        }
        catch (TaskCanceledException)
        {
        }
    }
    
    private async Task Close()
    {
        _cancellationTokenSource?.Cancel();
        IsVisible = false;
        await IsVisibleChanged.InvokeAsync(IsVisible);
    }

    public async ValueTask DisposeAsync()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        await Task.CompletedTask;
    }
}
";
}

