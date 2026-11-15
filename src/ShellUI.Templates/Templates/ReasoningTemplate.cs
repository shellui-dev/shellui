using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ReasoningTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "reasoning",
        DisplayName = "Reasoning",
        Description = "Collapsible block that streams a model's reasoning",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Reasoning.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "chat", "ai", "reasoning", "thinking", "collapsible" }
    };

    public static string Content => ComponentSource.Read("Components/Reasoning.razor");
}
