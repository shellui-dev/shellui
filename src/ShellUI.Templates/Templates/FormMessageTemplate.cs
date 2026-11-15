using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormMessageTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-message",
        DisplayName = "Form Message",
        Description = "Validation-aware error message for a form field",
        Category = ComponentCategory.Form,
        FilePath = "FormMessage.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "message", "validation" }
    };

    public static string Content => ComponentSource.Read("Components/FormMessage.razor");
}
