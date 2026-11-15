using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DataPickerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "data-picker",
        DisplayName = "DataPicker",
        Description = "Generic typed picker with search, keyboard nav, and templated rendering",
        Category = ComponentCategory.Form,
        FilePath = "DataPicker.razor"
    };

    public static string Content => ComponentSource.Read("Components/DataPicker.razor");
}
