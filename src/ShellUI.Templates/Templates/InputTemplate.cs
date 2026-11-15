using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class InputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "input",
        DisplayName = "Input",
        Description = "Accessible text input component with focus states",
        Category = ComponentCategory.Form,

        FilePath = "Input.razor",
        Dependencies = new List<string> { "input-variants" }
    };

    public static string Content => ComponentSource.Read("Components/Input.razor", variantsNamespace: true);
}

