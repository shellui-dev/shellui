using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-description",
        DisplayName = "Form Description",
        Description = "Helper text below a form control",
        Category = ComponentCategory.Form,
        FilePath = "FormDescription.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "description", "primitive" }
    };

    public static string Content => ComponentSource.Read("Components/FormDescription.razor");
}
