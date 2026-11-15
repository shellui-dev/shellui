using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsListTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs-list",
        DisplayName = "Tabs List",
        Description = "Container for tab triggers",
        Category = ComponentCategory.Navigation,

        FilePath = "TabsList.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/TabsList.razor");
}
