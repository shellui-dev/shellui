using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ChatMessageActionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chat-message-action",
        DisplayName = "Chat Message Action",
        Description = "Icon button for a chat message's actions bar",
        Category = ComponentCategory.DataDisplay,
        FilePath = "ChatMessageAction.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/ChatMessageAction.razor");
}
