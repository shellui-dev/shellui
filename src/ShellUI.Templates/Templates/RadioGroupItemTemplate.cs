using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class RadioGroupItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "radio-group-item",
        DisplayName = "Radio Group Item",
        Description = "Individual radio button within a Radio Group",
        Category = ComponentCategory.Form,
        FilePath = "RadioGroupItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "input", "radio", "item" }
    };

    public static string Content => ComponentSource.Read("Components/RadioGroupItem.razor");
}


