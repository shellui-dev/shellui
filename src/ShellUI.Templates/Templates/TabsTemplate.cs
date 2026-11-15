using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs",
        DisplayName = "Tabs",
        Description = "Tabbed navigation with TabsList, TabsTrigger, TabsContent (shadcn-style)",
        Category = ComponentCategory.Navigation,

        FilePath = "Tabs.razor",
        Dependencies = new List<string> { "tabs-list", "tabs-trigger", "tabs-content", "tab-models" }
    };

    public static string Content => ComponentSource.Read("Components/Tabs.razor");
}
