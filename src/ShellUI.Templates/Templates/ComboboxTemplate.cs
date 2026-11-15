using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ComboboxTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "combobox",
        DisplayName = "Combobox",
        Description = "Autocomplete input and command palette",
        Category = ComponentCategory.Form,

        FilePath = "Combobox.razor"
    };

    public static string Content => ComponentSource.Read("Components/Combobox.razor");
}

