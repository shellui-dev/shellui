using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ContextMenuContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "context-menu-content",
        DisplayName = "Context Menu Content",
        Description = "Menu panel for ContextMenu",
        Category = ComponentCategory.Overlay,
        FilePath = "ContextMenuContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "context-menu", "content" }
    };

    public static string Content => ComponentSource.Read("Components/ContextMenuContent.razor");
}
