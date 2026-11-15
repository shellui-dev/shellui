using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-group",
        DisplayName = "Command Group",
        Description = "Group of Command options with a heading",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandGroup.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "group" }
    };

    public static string Content => ComponentSource.Read("Components/CommandGroup.razor");
}
