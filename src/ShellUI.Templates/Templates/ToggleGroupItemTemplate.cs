using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ToggleGroupItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "toggle-group-item",
        DisplayName = "Toggle Group Item",
        Description = "Item for ToggleGroup",
        Category = ComponentCategory.Form,
        FilePath = "ToggleGroupItem.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "toggle", "group" }
    };

    public static string Content => ComponentSource.Read("Components/ToggleGroupItem.razor");
}
