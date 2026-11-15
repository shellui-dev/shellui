using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class RadioGroupTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "radio-group",
        DisplayName = "Radio Group",
        Description = "Radio button group with single selection",
        Category = ComponentCategory.Form,
        FilePath = "RadioGroup.razor",

        Tags = new List<string> { "form", "input", "radio", "selection" },
        Dependencies = new List<string> { "radio-group-item" }
    };

    public static string Content => ComponentSource.Read("Components/RadioGroup.razor");
}


