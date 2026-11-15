using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandInputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-input",
        DisplayName = "Command Input",
        Description = "Search box for a compositional Command",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandInput.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "input" }
    };

    public static string Content => ComponentSource.Read("Components/CommandInput.razor");
}
