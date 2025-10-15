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

<input 
    type=""@Type""
    value=""@Value""
    placeholder=""@Placeholder""
    disabled=""@Disabled""
    @oninput=""HandleInput""
    class=""flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50""
    @attributes=""AdditionalAttributes"" />

@code {
    [Parameter]
    public string Type { get; set; } = ""text"";

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

