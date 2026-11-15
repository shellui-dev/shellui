using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ResizablePanelTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "resizable-panel",
        DisplayName = "Resizable Panel",
        Description = "A panel inside a resizable panel group",
        Category = ComponentCategory.Layout,
        FilePath = "ResizablePanel.razor",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/ResizablePanel.razor");
}
