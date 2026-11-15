using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-trigger",
        DisplayName = "Menubar Trigger",
        Description = "Trigger button for the compositional Menubar pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menubar", "trigger" }
    };

    public static string Content => ComponentSource.Read("Components/MenubarTrigger.razor");
}
