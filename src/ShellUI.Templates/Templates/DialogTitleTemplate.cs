using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-title",
        DisplayName = "Dialog Title",
        Description = "Title for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogTitle.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/DialogTitle.razor");
}


