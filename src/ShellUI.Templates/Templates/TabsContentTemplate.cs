using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs-content",
        DisplayName = "Tabs Content",
        Description = "Tab content panel",
        Category = ComponentCategory.Navigation,

        FilePath = "TabsContent.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/TabsContent.razor");
}
