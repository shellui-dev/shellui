using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class KbdTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "kbd",
        DisplayName = "Kbd",
        Description = "Keyboard key hint for shortcuts",
        Category = ComponentCategory.Typography,
        FilePath = "Kbd.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "kbd", "keyboard", "shortcut" }
    };

    public static string Content => ComponentSource.Read("Components/Kbd.razor");
}
