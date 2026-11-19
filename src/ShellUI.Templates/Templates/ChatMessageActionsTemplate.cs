using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ChatMessageActionsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chat-message-actions",
        DisplayName = "Chat Message Actions",
        Description = "Copy and regenerate buttons for a chat message, plus your own",
        Category = ComponentCategory.DataDisplay,
        FilePath = "ChatMessageActions.razor",
        Dependencies = new List<string> { "copy-button", "chat-message-action" },
        Tags = new List<string> { "chat", "ai", "copy", "regenerate", "actions" }
    };

    public static string Content => ComponentSource.Read("Components/ChatMessageActions.razor");
}
