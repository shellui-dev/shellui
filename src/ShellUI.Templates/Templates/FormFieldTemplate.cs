using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormFieldTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-field",
        DisplayName = "Form Field",
        Description = "Generic field context primitive for the shadcn-style form pattern",
        Category = ComponentCategory.Form,
        FilePath = "FormField.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "field", "primitive" }
    };

    public static string Content => ComponentSource.Read("Components/FormField.razor");
}
