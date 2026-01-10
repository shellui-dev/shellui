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
        Version = "0.1.0",
        Dependencies = new List<string>(),
        IsAvailable = false
    };

    public static string Content => @"using System.Linq;

namespace YourProjectNamespace.Components.UI;

public static class Shell
{
    public static string Cn(params string?[] classes)
        => string.Join("" "", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
}
";
}