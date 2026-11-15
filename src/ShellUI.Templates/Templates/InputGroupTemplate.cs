using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class InputGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "input-group",
        DisplayName = "Input Group",
        Description = "Input with prefix and suffix slots for icons, units or buttons",
        Category = ComponentCategory.Form,
        FilePath = "InputGroup.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "input", "addon", "prefix", "suffix" }
    };

    public static string Content => ComponentSource.Read("Components/InputGroup.razor");
}
