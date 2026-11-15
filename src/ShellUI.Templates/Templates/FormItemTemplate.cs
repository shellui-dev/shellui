using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FormItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "form-item",
        DisplayName = "Form Item",
        Description = "Vertical spacing wrapper for a single form row",
        Category = ComponentCategory.Form,
        FilePath = "FormItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "item", "primitive" }
    };

    public static string Content => ComponentSource.Read("Components/FormItem.razor");
}
