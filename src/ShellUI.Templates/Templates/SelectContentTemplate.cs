using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SelectContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "select-content",
        DisplayName = "Select Content",
        Description = "Dropdown content for custom Select",
        Category = ComponentCategory.Form,
        FilePath = "SelectContent.razor",
        Dependencies = new List<string> { "select" },
        IsAvailable = false,
        Tags = new List<string> { "form", "select", "content" }
    };

    public static string Content => "";
}
