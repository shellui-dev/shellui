using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DropdownTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dropdown",
        DisplayName = "Dropdown",
        Description = "Dropdown menu with trigger and content",
        Category = ComponentCategory.Overlay,

        FilePath = "Dropdown.razor",
        Dependencies = new List<string> { "dropdown-trigger", "dropdown-content", "dropdown-item" }
    };

    public static string Content => ComponentSource.Read("Components/Dropdown.razor");
}


