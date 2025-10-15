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
        Variants = new List<string> { "default", "destructive", "success" },
        Tags = new List<string> { "notification", "toast", "feedback", "alert" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (IsVisible)
{
    <div class=""@(""fixed z-50 flex max-h-screen w-full flex-col-reverse gap-2 p-4 sm:flex-col md:max-w-[420px] "" + (Position == ""top-left"" ? ""top-0 left-0"" : Position == ""top-right"" ? ""top-0 right-0"" : Position == ""bottom-left"" ? ""bottom-0 left-0"" : ""bottom-0 right-0""))"">
        <div class=""@(""pointer-events-auto group relative flex w-full items-center justify-between space-x-4 overflow-hidden rounded-md border border-border p-6 pr-8 shadow-lg transition-all "" + (Variant == ""destructive"" ? ""border-destructive bg-destructive text-destructive-foreground"" : Variant == ""success"" ? ""border-green-500 bg-green-50 text-green-900 dark:bg-green-900/20 dark:text-green-100"" : ""bg-background text-foreground"") + "" "" + ClassName)""
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
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private async Task Close()
    {
        IsVisible = false;
        await IsVisibleChanged.InvokeAsync(IsVisible);
    }
}
";
}

