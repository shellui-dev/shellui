using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-header",
        DisplayName = "Dialog Header",
        Description = "Header for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogHeader.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/DialogHeader.razor");
}


