using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-description",
        DisplayName = "Dialog Description",
        Description = "Description for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogDescription.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/DialogDescription.razor");
}


