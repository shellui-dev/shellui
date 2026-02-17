using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SelectTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "select-trigger",
        DisplayName = "Select Trigger",
        Description = "Trigger button for custom Select",
        Category = ComponentCategory.Form,
        FilePath = "SelectTrigger.razor",
        Dependencies = new List<string> { "select" },
        IsAvailable = false,
        Tags = new List<string> { "form", "select", "trigger" }
    };

    public static string Content => "";
}
