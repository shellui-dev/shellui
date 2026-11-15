using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DropdownTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dropdown-trigger",
        DisplayName = "Dropdown Trigger",
        Description = "Trigger for Dropdown (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "DropdownTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "dropdown", "trigger" }
    };

    public static string Content => ComponentSource.Read("Components/DropdownTrigger.razor");
}
