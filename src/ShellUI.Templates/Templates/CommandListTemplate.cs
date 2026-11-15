using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandListTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-list",
        DisplayName = "Command List",
        Description = "Scrollable list of Command options",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandList.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "list" }
    };

    public static string Content => ComponentSource.Read("Components/CommandList.razor");
}
