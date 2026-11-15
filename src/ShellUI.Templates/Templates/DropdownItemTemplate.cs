using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DropdownItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dropdown-item",
        DisplayName = "Dropdown Item",
        Description = "Menu item for Dropdown (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "DropdownItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "dropdown", "item", "menu" }
    };

    public static string Content => ComponentSource.Read("Components/DropdownItem.razor");
}
