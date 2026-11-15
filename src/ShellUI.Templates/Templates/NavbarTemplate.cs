using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NavbarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "navbar",
        DisplayName = "Navbar",
        Description = "Top navigation bar with backdrop blur",
        Category = ComponentCategory.Layout,

        FilePath = "Navbar.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Navbar.razor");
}


