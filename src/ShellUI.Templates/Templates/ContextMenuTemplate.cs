using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ContextMenuTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "context-menu",
        DisplayName = "Context Menu",
        Description = "A context menu component that appears on right-click",
        Category = ComponentCategory.Navigation,
        FilePath = "ContextMenu.razor",
        Tags = new List<string> { "menu", "context", "right-click", "dropdown" },
        Dependencies = new List<string> { "context-menu-models", "context-menu-trigger", "context-menu-content", "context-menu-option" }
    };

    public static string Content => ComponentSource.Read("Components/ContextMenu.razor");
}

