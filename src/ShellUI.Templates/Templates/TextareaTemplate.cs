using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TextareaTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "textarea",
        DisplayName = "Textarea",
        Description = "Multi-line text input component",
        Category = ComponentCategory.Form,

        FilePath = "Textarea.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<textarea 
    value=""@Value""
    placeholder=""@Placeholder""
    disabled=""@Disabled""
    @oninput=""HandleInput""
    class=""flex min-h-[80px] w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50""
    @attributes=""AdditionalAttributes""></textarea>

@code {
    [Parameter]
    public string? Value { get; set; }

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task HandleInput(ChangeEventArgs e)
    {
        Value = e.Value?.ToString();
        await ValueChanged.InvokeAsync(Value);
    }
}
";
}


