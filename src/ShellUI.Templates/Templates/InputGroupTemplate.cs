using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class InputGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "input-group",
        DisplayName = "Input Group",
        Description = "Input with prefix and suffix slots for icons, units or buttons",
        Category = ComponentCategory.Form,
        FilePath = "InputGroup.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "input", "addon", "prefix", "suffix" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""flex h-10 w-full items-center rounded-md border border-input bg-background text-sm ring-offset-background focus-within:ring-2 focus-within:ring-ring focus-within:ring-offset-2"", Disabled ? ""cursor-not-allowed opacity-50"" : """", Class)"">
    @if (Prefix != null)
    {
        <span class=""flex shrink-0 items-center pl-3 text-muted-foreground [&>svg]:size-4"">@Prefix</span>
    }
    <input type=""@Type""
           value=""@Value""
           placeholder=""@Placeholder""
           disabled=""@Disabled""
           @oninput=""HandleInput""
           class=""h-full min-w-0 flex-1 bg-transparent px-3 py-2 placeholder:text-muted-foreground focus:outline-none disabled:cursor-not-allowed""
           @attributes=""AdditionalAttributes"" />
    @if (Suffix != null)
    {
        <span class=""flex shrink-0 items-center pr-3 text-muted-foreground [&>svg]:size-4"">@Suffix</span>
    }
</div>

@code {
    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback<string?> ValueChanged { get; set; }
    [Parameter] public string Type { get; set; } = ""text"";
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public RenderFragment? Prefix { get; set; }
    [Parameter] public RenderFragment? Suffix { get; set; }
    [Parameter] public string? Class { get; set; }
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
