using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SwitchTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "switch",
        DisplayName = "Switch",
        Description = "Toggle switch for boolean values",
        Category = ComponentCategory.Form,

        FilePath = "Switch.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Switch.razor");
}


