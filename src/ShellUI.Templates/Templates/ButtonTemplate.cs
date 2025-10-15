using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ButtonTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "button",
        DisplayName = "Button",
        Description = "Interactive button component with multiple variants and sizes",
        Category = ComponentCategory.Form,
        FilePath = "Button.razor",
        Version = "0.1.0",
        Variants = new List<string> { "default", "destructive", "outline", "secondary", "ghost", "link" },
        Tags = new List<string> { "form", "input", "interactive", "button", "action" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<button 
    type=""@Type""
    disabled=""@(Disabled || IsLoading)""
    class=""inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 @(Variant == ""destructive"" ? ""bg-destructive text-destructive-foreground hover:bg-destructive/90"" : Variant == ""outline"" ? ""border border-input bg-background hover:bg-accent hover:text-accent-foreground"" : Variant == ""secondary"" ? ""bg-secondary text-secondary-foreground hover:bg-secondary/80"" : Variant == ""ghost"" ? ""hover:bg-accent hover:text-accent-foreground"" : Variant == ""link"" ? ""text-primary underline-offset-4 hover:underline"" : ""bg-primary text-primary-foreground hover:bg-primary/90"") @(Size == ""sm"" ? ""h-9 rounded-md px-3"" : Size == ""lg"" ? ""h-11 rounded-md px-8"" : Size == ""icon"" ? ""h-10 w-10"" : ""h-10 px-4 py-2"")""
    @onclick=""HandleClick""
    @attributes=""AdditionalAttributes"">
    @if (IsLoading)
    {
        <svg class=""animate-spin -ml-1 mr-3 h-5 w-5"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"">
            <circle class=""opacity-25"" cx=""12"" cy=""12"" r=""10"" stroke=""currentColor"" stroke-width=""4""></circle>
            <path class=""opacity-75"" fill=""currentColor"" d=""M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z""></path>
        </svg>
    }
    @ChildContent
</button>

@code {
    [Parameter] 
    public string Variant { get; set; } = ""default"";

    [Parameter] 
    public string Size { get; set; } = ""md"";

    [Parameter] 
    public string Type { get; set; } = ""button"";

    [Parameter] 
    public bool Disabled { get; set; }

    [Parameter] 
    public bool IsLoading { get; set; }

    [Parameter] 
    public RenderFragment? ChildContent { get; set; }

    [Parameter] 
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task HandleClick(MouseEventArgs args)
    {
        if (!Disabled && !IsLoading)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
";
}

