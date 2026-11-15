using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandEmptyTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-empty",
        DisplayName = "Command Empty",
        Description = "Shown when no Command option matches the search",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandEmpty.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "empty" }
    };

    public static string Content => ComponentSource.Read("Components/CommandEmpty.razor");
}
