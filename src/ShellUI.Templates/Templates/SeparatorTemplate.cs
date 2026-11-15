using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "separator",
        DisplayName = "Separator",
        Description = "Horizontal or vertical divider line",
        Category = ComponentCategory.Layout,

        FilePath = "Separator.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Separator.razor");
}


