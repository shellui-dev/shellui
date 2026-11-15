using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormControlTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-control",
        DisplayName = "Form Control",
        Description = "Wrapper around the input control for the shadcn-style form pattern",
        Category = ComponentCategory.Form,
        FilePath = "FormControl.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "control", "primitive" }
    };

    public static string Content => ComponentSource.Read("Components/FormControl.razor");
}
