using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ContextMenuModelsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "context-menu-models",
        DisplayName = "ContextMenu Models",
        Description = "Models for ContextMenu component",
        Category = ComponentCategory.Navigation,
        FilePath = "Models/ContextMenuModels.cs",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Models/ContextMenuModels.cs");
}
