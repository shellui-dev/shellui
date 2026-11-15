using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-footer",
        DisplayName = "Dialog Footer",
        Description = "Footer for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogFooter.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/DialogFooter.razor");
}


