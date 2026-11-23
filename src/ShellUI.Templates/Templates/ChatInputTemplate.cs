using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ChatInputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chat-input",
        DisplayName = "Chat Input",
        Description = "Prompt input that sends on Enter, adds a newline on Shift+Enter, and can attach files",
        Category = ComponentCategory.Form,
        FilePath = "ChatInput.razor",
        Dependencies = new List<string> { "chat-models" },
        Tags = new List<string> { "chat", "ai", "prompt", "input", "attachments" }
    };

    public static string Content => ComponentSource.Read("Components/ChatInput.razor");
}
