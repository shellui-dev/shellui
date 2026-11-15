using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CommandTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command",
        DisplayName = "Command",
        Description = "Command palette component for quick actions",
        Category = ComponentCategory.Overlay,
        FilePath = "Command.razor",
        Dependencies = new List<string> { "command-models", "command-input", "command-list", "command-group", "command-option", "command-empty", "command-separator" }
    };

    public static string Content => ComponentSource.Read("Components/Command.razor");
}

