using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CollapsibleTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "collapsible-trigger",
        DisplayName = "Collapsible Trigger",
        Description = "Trigger for Collapsible component",
        Category = ComponentCategory.Layout,
        FilePath = "CollapsibleTrigger.razor",
        Dependencies = new List<string>(),
        IsAvailable = false,
        Tags = new List<string> { "layout", "collapsible", "trigger", "toggle" }
    };

    public static string Content => ComponentSource.Read("Components/CollapsibleTrigger.razor");
}
