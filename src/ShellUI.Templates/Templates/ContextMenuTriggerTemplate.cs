using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ContextMenuTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "context-menu-trigger",
        DisplayName = "Context Menu Trigger",
        Description = "Right-click trigger area for ContextMenu",
        Category = ComponentCategory.Overlay,
        FilePath = "ContextMenuTrigger.razor",
        Dependencies = new List<string> { "context-menu" },
        IsAvailable = false,
        Tags = new List<string> { "overlay", "context-menu", "trigger" }
    };

    public static string Content => "";
}
