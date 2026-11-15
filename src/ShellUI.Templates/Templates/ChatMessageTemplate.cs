using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ChatMessageTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chat-message",
        DisplayName = "Chat Message",
        Description = "Chat bubble for user, assistant or system messages with streaming indicator",
        Category = ComponentCategory.DataDisplay,
        FilePath = "ChatMessage.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "chat", "ai", "message" }
    };

    public static string Content => ComponentSource.Read("Components/ChatMessage.razor");
}
