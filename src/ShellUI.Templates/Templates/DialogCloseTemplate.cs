using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogCloseTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-close",
        DisplayName = "Dialog Close",
        Description = "Close button for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogClose.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/DialogClose.razor");
}


