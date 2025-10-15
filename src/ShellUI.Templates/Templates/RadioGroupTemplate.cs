using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class RadioGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "radio-group",
        DisplayName = "Radio Group",
        Description = "Radio button group with single selection",
        Category = ComponentCategory.Form,
        FilePath = "RadioGroup.razor",
        Version = "0.1.0",
        Tags = new List<string> { "form", "input", "radio", "selection" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@ClassName"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter]
    public string? Value { get; set; }
    
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    public async Task SetValue(string value)
    {
        if (Value != value)
        {
            Value = value;
            await ValueChanged.InvokeAsync(value);
        }
    }
}
";
}

