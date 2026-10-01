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

        Tags = new List<string> { "form", "validation", "input", "wrapper" },
        Dependencies = new List<string> { "label", "input", "button", "form-field", "form-item", "form-label", "form-control", "form-description", "form-message" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using Microsoft.AspNetCore.Components.Forms

<form @onsubmit=""HandleSubmit"" @attributes=""AdditionalAttributes"" class=""@Shell.Cn(ClassName, Class)"" novalidate>
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
    public string? Class { get; set; }

    // Deprecated: use Class.
    [Parameter]
    public string ClassName { get; set; } = ""space-y-6"";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    // For validation, use EditForm with DataAnnotationsValidator; FormField and FormMessage work inside it.
    private async Task HandleSubmit()
    {
        if (OnValidSubmit.HasDelegate) await OnValidSubmit.InvokeAsync();
    }
}

";
}


