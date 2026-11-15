using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ChatInputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chat-input",
        DisplayName = "Chat Input",
        Description = "Prompt input that sends on Enter and adds a newline on Shift+Enter",
        Category = ComponentCategory.Form,
        FilePath = "ChatInput.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "chat", "ai", "prompt", "input" }
    };

    public static string Content => ComponentSource.Read("Components/ChatInput.razor");
}
