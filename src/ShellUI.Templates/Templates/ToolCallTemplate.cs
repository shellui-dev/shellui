using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ToolCallTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tool-call",
        DisplayName = "Tool Call",
        Description = "Collapsible card with a tool's name, arguments, result and running, done or error state",
        Category = ComponentCategory.DataDisplay,
        FilePath = "ToolCall.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "chat", "ai", "tool", "function-call" }
    };

    public static string Content => ComponentSource.Read("Components/ToolCall.razor");
}
