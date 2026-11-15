using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogActionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-action",
        DisplayName = "Alert Dialog Action",
        Description = "Confirm action button for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogAction.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "action" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialogAction.razor");
}
