using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ButtonTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "button",
        DisplayName = "Button",
        Description = "Interactive button component with multiple variants and sizes",
        Category = ComponentCategory.Form,
        FilePath = "Button.razor",
        Dependencies = new List<string> { "button-variants" },
        Variants = new List<string> { "default", "destructive", "outline", "secondary", "ghost", "link" },
        Tags = new List<string> { "form", "input", "interactive", "button", "action" }
    };

    public static string Content => ComponentSource.Read("Components/Button.razor", variantsNamespace: true);
}

