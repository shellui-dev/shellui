using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ChatModelsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "chat-models",
        DisplayName = "Chat Models",
        Description = "ChatSubmission, the text and files ChatInput sends",
        Category = ComponentCategory.Form,
        FilePath = "Models/ChatModels.cs",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Models/ChatModels.cs");
}
