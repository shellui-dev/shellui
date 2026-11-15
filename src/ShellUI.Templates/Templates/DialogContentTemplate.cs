using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-content",
        DisplayName = "Dialog Content",
        Description = "Content container for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogContent.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/DialogContent.razor");
}
