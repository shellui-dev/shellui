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

    public static string Content => @"namespace YourProjectNamespace.Components.Models;

public class ContextMenuItem
{
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Shortcut { get; set; }
    public bool Disabled { get; set; }
    public bool IsSeparator { get; set; }
    public object? Data { get; set; }
}
";
}
