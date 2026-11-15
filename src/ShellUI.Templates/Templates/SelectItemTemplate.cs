using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SelectItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "select-item",
        DisplayName = "Select Item",
        Description = "Option item for custom Select",
        Category = ComponentCategory.Form,
        FilePath = "SelectItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "select", "item", "option" }
    };

    public static string Content => ComponentSource.Read("Components/SelectItem.razor");
}
