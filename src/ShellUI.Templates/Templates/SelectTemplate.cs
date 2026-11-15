using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SelectTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "select",
        DisplayName = "Select",
        Description = "Dropdown select input component",
        Category = ComponentCategory.Form,

        FilePath = "Select.razor",
        Dependencies = new List<string> { "select-trigger", "select-content", "select-item" }
    };

    public static string Content => ComponentSource.Read("Components/Select.razor");
}


