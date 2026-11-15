using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class MultiSelectTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "multi-select",
        DisplayName = "MultiSelect",
        Description = "Generic multi-value picker with chip display and search",
        Category = ComponentCategory.Form,
        FilePath = "MultiSelect.razor"
    };

    public static string Content => ComponentSource.Read("Components/MultiSelect.razor");
}
