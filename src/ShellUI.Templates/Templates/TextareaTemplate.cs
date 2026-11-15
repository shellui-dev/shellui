using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TextareaTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "textarea",
        DisplayName = "Textarea",
        Description = "Multi-line text input component",
        Category = ComponentCategory.Form,

        FilePath = "Textarea.razor",
        Dependencies = new List<string> { "textarea-variants" }
    };

    public static string Content => ComponentSource.Read("Components/Textarea.razor", variantsNamespace: true);
}


