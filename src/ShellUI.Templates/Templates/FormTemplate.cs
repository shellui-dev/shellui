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

    public static string Content => ComponentSource.Read("Components/Form.razor");
}


