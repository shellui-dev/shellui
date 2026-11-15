using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs-trigger",
        DisplayName = "Tabs Trigger",
        Description = "Tab trigger button",
        Category = ComponentCategory.Navigation,

        FilePath = "TabsTrigger.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/TabsTrigger.razor");
}
