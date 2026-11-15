using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ResizableTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "resizable",
        DisplayName = "Resizable",
        Description = "Resizable panel group with draggable, keyboard-accessible handles",
        Category = ComponentCategory.Layout,
        FilePath = "ResizablePanelGroup.razor",
        Dependencies = new List<string> { "resizable-panel", "resizable-handle", "shellui-js" },
        Tags = new List<string> { "layout", "resizable", "panel", "split" }
    };

    public static string Content => ComponentSource.Read("Components/ResizablePanelGroup.razor");
}
