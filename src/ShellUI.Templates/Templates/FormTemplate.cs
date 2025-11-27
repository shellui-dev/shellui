using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form",
        DisplayName = "Form",
        Description = "Form wrapper component with validation support",
        Category = ComponentCategory.Form,
        FilePath = "Form.razor",
        Version = "0.1.0",
        Tags = new List<string> { "form", "validation", "input", "wrapper" },
        Dependencies = new List<string> { "label", "input", "button" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using Microsoft.AspNetCore.Components.Forms

<form @onsubmit=""HandleSubmit"" @attributes=""AdditionalAttributes"" class=""@ClassName"">
    @ChildContent
</form>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public EventCallback OnValidSubmit { get; set; }
    
    [Parameter]
    public EventCallback OnInvalidSubmit { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = ""space-y-6"";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private async Task HandleSubmit()
    {
        await Task.CompletedTask;
    }
}
";
}

