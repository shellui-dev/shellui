using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class InputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "input",
        DisplayName = "Input",
        Description = "Accessible text input component with focus states",
        Category = ComponentCategory.Form,
        Version = "0.1.0",
        FilePath = "Input.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants

<input 
    type=""@Type""
    value=""@Value""
    placeholder=""@Placeholder""
    disabled=""@Disabled""
    readonly=""@ReadOnly""
    class=""@ComputedClass""
    @attributes=""AdditionalAttributes""
    @oninput=""HandleInput""
    @onfocus=""HandleFocus""
    @onblur=""HandleBlur"" />

@code {
    [Parameter] public InputVariant Variant { get; set; } = InputVariant.Default;
    [Parameter] public string Type { get; set; } = ""text"";
    [Parameter] public string Value { get; set; } = """";
    [Parameter] public string Placeholder { get; set; } = """";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool ReadOnly { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }
    [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string ComputedClass => InputVariants.Get(Variant, Class);

    private async Task HandleInput(ChangeEventArgs e)
    {
        var newValue = e.Value?.ToString() ?? """";
        Value = newValue;
        await ValueChanged.InvokeAsync(newValue);
    }

    private async Task HandleFocus(FocusEventArgs e)
    {
        await OnFocus.InvokeAsync(e);
    }

    private async Task HandleBlur(FocusEventArgs e)
    {
        await OnBlur.InvokeAsync(e);
    }

    public enum InputVariant { Default, Error }

    public static class InputVariants
    {
        public static string Get(InputVariant variant, string? className)
        {
            var baseClasses = ""flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"";
            
            var variantClasses = variant switch
            {
                InputVariant.Error => ""border-destructive focus-visible:ring-destructive"",
                _ => """"
            };

            return Shell.Cn(baseClasses, variantClasses, className);
        }
    }
}
";
}
