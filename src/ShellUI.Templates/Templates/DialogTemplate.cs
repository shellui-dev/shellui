using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog",
        DisplayName = "Dialog",
        Description = "Modal dialog component",
        Category = ComponentCategory.Overlay,
        Version = "0.1.0",
        FilePath = "Dialog.razor",
        Dependencies = new List<string> { "button" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (IsOpen)
{
    <div class=""fixed inset-0 z-50 flex items-center justify-center"">
        <!-- Backdrop with blur -->
        <div class=""fixed inset-0 bg-black/50 backdrop-blur-sm"" @onclick=""Close"" />

        <!-- Dialog -->
        <div class=""relative z-50 w-full max-w-lg rounded-lg border border-border bg-card text-card-foreground p-6 shadow-lg"">
            @if (Title != null || Description != null)
            {
                <div class=""flex flex-col space-y-1.5 text-center sm:text-left"">
                    @if (Title != null)
                    {
                        <h2 class=""text-lg font-semibold leading-none tracking-tight"">
                            @Title
                        </h2>
                    }
                    @if (Description != null)
                    {
                        <p class=""text-sm text-muted-foreground"">
                            @Description
                        </p>
                    }
                </div>
            }

            <div class=""mt-4"">
                @ChildContent
            </div>

            @if (Footer != null)
            {
                <div class=""mt-6 flex flex-col-reverse sm:flex-row sm:justify-end sm:space-x-2"">
                    @Footer
                </div>
            }

            <!-- Close button -->
            <button
                @onclick=""Close""
                class=""absolute right-4 top-4 rounded-sm opacity-70 ring-offset-background transition-opacity hover:opacity-100 focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"">
                <svg class=""h-4 w-4"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                    <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M6 18L18 6M6 6l12 12"" />
                </svg>
            </button>
        </div>
    </div>
}

@code {
    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public RenderFragment? Footer { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    private async Task Close()
    {
        IsOpen = false;
        await OnClose.InvokeAsync();
    }
}
";
}

