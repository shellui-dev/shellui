using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ContextMenuOptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "context-menu-option",
        DisplayName = "Context Menu Option",
        Description = "Menu item for ContextMenu (compositional)",
        Category = ComponentCategory.Overlay,
        FilePath = "ContextMenuOption.razor",
        Dependencies = new List<string> { "context-menu" },
        IsAvailable = false,
        Tags = new List<string> { "overlay", "context-menu", "option", "item" }
    };

    public static string Content => "";
}
