using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DropdownContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dropdown-content",
        DisplayName = "Dropdown Content",
        Description = "Content panel for Dropdown (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "DropdownContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "dropdown", "content" }
    };

    public static string Content => ComponentSource.Read("Components/DropdownContent.razor");
}
