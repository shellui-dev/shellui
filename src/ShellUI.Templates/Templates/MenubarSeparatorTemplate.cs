using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarSeparatorTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-separator",
        DisplayName = "Menubar Separator",
        Description = "Horizontal divider for the Menubar dropdown",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarSeparator.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menubar", "separator" }
    };

    public static string Content => ComponentSource.Read("Components/MenubarSeparator.razor");
}
