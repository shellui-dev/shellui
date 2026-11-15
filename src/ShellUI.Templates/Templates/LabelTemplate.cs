using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class LabelTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "label",
        DisplayName = "Label",
        Description = "Form label component",
        Category = ComponentCategory.Form,

        FilePath = "Label.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Label.razor");
}


