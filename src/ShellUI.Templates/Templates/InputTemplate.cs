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

        FilePath = "Input.razor",
        Dependencies = new List<string> { "input-variants" }
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
}
";
}

