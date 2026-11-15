using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-footer",
        DisplayName = "Alert Dialog Footer",
        Description = "Footer subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogFooter.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "footer" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialogFooter.razor");
}
