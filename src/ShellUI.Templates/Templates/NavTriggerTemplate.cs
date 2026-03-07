using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "nav-trigger",
        DisplayName = "Nav Trigger",
        Description = "Trigger button for NavItem dropdown",
        Category = ComponentCategory.Navigation,
        FilePath = "NavTrigger.razor",
        Dependencies = new List<string> { "nav-item" },
        IsAvailable = false,
        Tags = new List<string> { "navigation", "nav", "trigger" }
    };

    public static string Content => "";
}
