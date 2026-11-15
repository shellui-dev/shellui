using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class PopoverTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "popover-trigger",
        DisplayName = "Popover Trigger",
        Description = "Trigger for Popover (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "PopoverTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "popover", "trigger" }
    };

    public static string Content => ComponentSource.Read("Components/PopoverTrigger.razor");
}
