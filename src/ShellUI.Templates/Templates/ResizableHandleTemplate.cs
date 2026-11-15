using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ResizableHandleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "resizable-handle",
        DisplayName = "Resizable Handle",
        Description = "Draggable handle between two resizable panels",
        Category = ComponentCategory.Layout,
        FilePath = "ResizableHandle.razor",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/ResizableHandle.razor");
}
