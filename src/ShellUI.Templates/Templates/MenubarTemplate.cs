using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar",
        DisplayName = "Menubar",
        Description = "Application menubar component",
        Category = ComponentCategory.Navigation,
        FilePath = "Menubar.razor",

        Tags = new List<string> { "navigation", "menu", "menubar", "app" },
        Dependencies = new List<string> { "menubar-item", "menubar-menu", "menubar-trigger", "menubar-content", "menubar-separator" }
    };

    public static string Content => ComponentSource.Read("Components/Menubar.razor");
}


