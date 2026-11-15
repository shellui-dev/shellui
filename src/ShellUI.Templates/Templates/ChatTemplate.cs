using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ChatTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chat",
        DisplayName = "Chat",
        Description = "Chat panel with header, auto-scrolling message list and footer",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Chat.razor",
        Dependencies = new List<string> { "chat-message", "chat-input" },
        Tags = new List<string> { "chat", "ai", "conversation" }
    };

    public static string Content => ComponentSource.Read("Components/Chat.razor");
}
