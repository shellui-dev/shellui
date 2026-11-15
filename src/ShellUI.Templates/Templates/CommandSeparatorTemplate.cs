using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandSeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-separator",
        DisplayName = "Command Separator",
        Description = "Divider between Command groups",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandSeparator.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "separator" }
    };

    public static string Content => ComponentSource.Read("Components/CommandSeparator.razor");
}
