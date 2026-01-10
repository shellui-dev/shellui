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
        Dependencies = new List<string> { "button-variants" },
        Variants = new List<string> { "default", "destructive", "outline", "secondary", "ghost", "link" },
        Tags = new List<string> { "form", "input", "interactive", "button", "action" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants

<button 
    type=""@Type""
    disabled=""@(Disabled || IsLoading)""
    class=""@ComputedClass""
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
    [Parameter] public ButtonVariant Variant { get; set; } = ButtonVariant.Default;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Default;
    [Parameter] public string Type { get; set; } = ""button"";
    [Parameter] public string? Class { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string ComputedClass => ButtonVariants.Get(Variant, Size, Class);

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
