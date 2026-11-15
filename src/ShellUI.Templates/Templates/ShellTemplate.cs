using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ShellTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "shell",
        DisplayName = "Shell Utilities",
        Description = "ShellUI utility classes and helpers",
        Category = ComponentCategory.Utility,
        FilePath = "Shell.cs",

        Dependencies = new List<string>(),
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Shell.cs");
}
