using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TagInputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tag-input",
        DisplayName = "TagInput",
        Description = "Free-form tag input with chip display and paste splitting",
        Category = ComponentCategory.Form,
        FilePath = "TagInput.razor"
    };

    public static string Content => ComponentSource.Read("Components/TagInput.razor");
}
