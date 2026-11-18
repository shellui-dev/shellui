using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CodeBlockTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "code-block",
        DisplayName = "Code Block",
        Description = "Code with a language label, optional filename and a copy button",
        Category = ComponentCategory.DataDisplay,
        FilePath = "CodeBlock.razor",
        Dependencies = new List<string> { "copy-button" },
        Tags = new List<string> { "code", "snippet", "copy", "pre" }
    };

    public static string Content => ComponentSource.Read("Components/CodeBlock.razor");
}
