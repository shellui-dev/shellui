using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormLabelTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-label",
        DisplayName = "Form Label",
        Description = "Styled label for the shadcn-style form pattern",
        Category = ComponentCategory.Form,
        FilePath = "FormLabel.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "label", "primitive" }
    };

    public static string Content => ComponentSource.Read("Components/FormLabel.razor");
}
