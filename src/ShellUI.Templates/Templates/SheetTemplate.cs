using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet",
        DisplayName = "Sheet",
        Description = "Side panel/drawer component with multiple positions",
        Category = ComponentCategory.Overlay,
        FilePath = "Sheet.razor",
        Version = "0.1.0",
        Tags = new List<string> { "overlay", "sheet", "drawer", "panel", "side" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (IsOpen)
{
    <div class=""fixed inset-0 z-50 flex"" @onclick=""Close"">
        <div class=""fixed inset-0 bg-black/50 backdrop-blur-sm""></div>
        
        <div @onclick:stopPropagation=""true""
             class=""@(""fixed z-50 bg-card text-card-foreground shadow-lg border "" + (Side == ""left"" ? ""left-0 top-0 h-full w-80 animate-slide-in-from-left"" : """") + (Side == ""right"" ? ""right-0 top-0 h-full w-80 animate-slide-in-from-right"" : """") + (Side == ""top"" ? ""top-0 left-0 w-full h-80 animate-slide-in-from-top"" : """") + (Side == ""bottom"" ? ""bottom-0 left-0 w-full h-80 animate-slide-in-from-bottom"" : """") + "" "" + ClassName)""
             @attributes=""AdditionalAttributes"">
            <div class=""flex flex-col h-full"">
                <div class=""flex items-center justify-between p-6 border-b"">
                    <h2 class=""text-lg font-semibold"">@Title</h2>
                    <button type=""button"" @onclick=""Close"" class=""rounded-sm opacity-70 ring-offset-background transition-opacity hover:opacity-100"">
                        <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M6 18L18 6M6 6l12 12"" />
                        </svg>
                    </button>
                </div>
                <div class=""flex-1 overflow-auto p-6"">
                    @ChildContent
                </div>
            </div>
        </div>
    </div>
}

@code {
    [Parameter]
    public bool IsOpen { get; set; }
    
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }
    
    [Parameter]
    public string Title { get; set; } = ""Sheet"";
    
    [Parameter]
    public string Side { get; set; } = ""right"";
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private async Task Close()
    {
        IsOpen = false;
        await IsOpenChanged.InvokeAsync(false);
    }
}
";
}

