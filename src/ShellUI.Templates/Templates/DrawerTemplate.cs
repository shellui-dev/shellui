using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer",
        DisplayName = "Drawer",
        Description = "Sliding drawer component with handle",
        Category = ComponentCategory.Overlay,
        FilePath = "Drawer.razor",
        Version = "0.1.0",
        Tags = new List<string> { "overlay", "drawer", "modal", "slide" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (IsOpen)
{
    <div class=""fixed inset-0 z-50"" @onclick=""Close"">
        <div class=""fixed inset-0 bg-black/50 backdrop-blur-sm""></div>
        
        <div @onclick:stopPropagation=""true""
             class=""@(""fixed bottom-0 left-0 right-0 z-50 mx-auto max-w-md rounded-t-2xl border bg-card text-card-foreground shadow-lg animate-slide-up "" + ClassName)""
             @attributes=""AdditionalAttributes"">
            <div class=""flex flex-col h-auto max-h-[85vh]"">
                <div class=""flex items-center justify-center p-4 border-b"">
                    <div class=""w-12 h-1.5 bg-muted rounded-full""></div>
                </div>
                @if (!string.IsNullOrEmpty(Title))
                {
                    <div class=""px-6 pt-4"">
                        <h2 class=""text-lg font-semibold"">@Title</h2>
                        @if (!string.IsNullOrEmpty(Description))
                        {
                            <p class=""text-sm text-muted-foreground mt-1"">@Description</p>
                        }
                    </div>
                }
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
    public string? Title { get; set; }
    
    [Parameter]
    public string? Description { get; set; }
    
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

