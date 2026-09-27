using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CommandModelsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-models",
        DisplayName = "Command Models",
        Description = "Models for Command and CommandPalette",
        Category = ComponentCategory.Overlay,
        FilePath = "Models/CommandModels.cs",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => @"namespace YourProjectNamespace.Components.Models;

public class CommandItem
{
    public string Title { get; set; } = """";
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Shortcut { get; set; }
    public string? Group { get; set; }
    public Func<Task>? Action { get; set; }
}

";
}
