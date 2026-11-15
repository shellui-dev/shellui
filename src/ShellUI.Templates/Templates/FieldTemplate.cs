using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class FieldTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "field",
        DisplayName = "Field",
        Description = "Label, control, description and validation message as one unit",
        Category = ComponentCategory.Form,
        FilePath = "Field.razor",
        Dependencies = new List<string> { "form-message" },
        Tags = new List<string> { "form", "field", "label", "validation" }
    };

    public static string Content => ComponentSource.Read("Components/Field.razor");
}
