using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ToggleGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "toggle-group",
        DisplayName = "Toggle Group",
        Description = "Single or multiple selection toggle buttons",
        Category = ComponentCategory.Form,
        FilePath = "ToggleGroup.razor",
        Dependencies = new List<string> { "toggle-group-item" },
        Tags = new List<string> { "toggle", "group", "segmented" }
    };

    public static string Content => ComponentSource.Read("Components/ToggleGroup.razor");
}
