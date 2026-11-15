using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogCancelTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-cancel",
        DisplayName = "Alert Dialog Cancel",
        Description = "Cancel button for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogCancel.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "cancel" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialogCancel.razor");
}
