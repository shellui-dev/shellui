using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ButtonGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "button-group",
        DisplayName = "Button Group",
        Description = "Joins adjacent buttons into a single horizontal or vertical group",
        Category = ComponentCategory.Form,
        FilePath = "ButtonGroup.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "button", "group", "toolbar" }
    };

    public static string Content => ComponentSource.Read("Components/ButtonGroup.razor");
}
