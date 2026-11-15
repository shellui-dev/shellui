using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TypedSelectTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "typed-select",
        DisplayName = "TypedSelect",
        Description = "Generic <select> that binds to any type — string, enum (nullable), Guid, bool, int, decimal — via InvariantCulture round-tripping. No external font dependency.",
        Category = ComponentCategory.Form,
        FilePath = "TypedSelect.razor"
    };

    public static string Content => ComponentSource.Read("Components/TypedSelect.razor");
}
