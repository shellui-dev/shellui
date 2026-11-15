using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandOptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-option",
        DisplayName = "Command Option",
        Description = "Selectable option in a compositional Command",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandOption.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "option" }
    };

    public static string Content => ComponentSource.Read("Components/CommandOption.razor");
}
