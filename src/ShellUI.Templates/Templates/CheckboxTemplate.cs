using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CheckboxTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "checkbox",
        DisplayName = "Checkbox",
        Description = "Checkbox input with checked state",
        Category = ComponentCategory.Form,

        FilePath = "Checkbox.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Checkbox.razor");
}


