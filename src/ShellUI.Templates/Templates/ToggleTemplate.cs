using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ToggleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "toggle",
        DisplayName = "Toggle",
        Description = "Toggle button with pressed state",
        Category = ComponentCategory.Form,
        FilePath = "Toggle.razor",

        Dependencies = new List<string> { "toggle-variants" },
        Tags = new List<string> { "form", "toggle", "button" }
    };

    public static string Content => ComponentSource.Read("Components/Toggle.razor");
}

