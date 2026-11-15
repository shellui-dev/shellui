using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class PromptSuggestionsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "prompt-suggestions",
        DisplayName = "Prompt Suggestions",
        Description = "Clickable suggestion chips for an empty chat",
        Category = ComponentCategory.DataDisplay,
        FilePath = "PromptSuggestions.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "chat", "ai", "suggestions", "prompt" }
    };

    public static string Content => ComponentSource.Read("Components/PromptSuggestions.razor");
}
