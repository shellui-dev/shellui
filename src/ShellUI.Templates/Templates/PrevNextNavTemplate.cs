using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class PrevNextNavTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "prev-next-nav",
        DisplayName = "Prev/Next Navigation",
        Description = "Previous and next page navigation for documentation",
        Category = ComponentCategory.Navigation,
        FilePath = "PrevNextNav.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "docs", "navigation", "prev", "next", "pagination" }
    };

    public static string Content => ComponentSource.Read("Components/PrevNextNav.razor");
}
