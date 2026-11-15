using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-header",
        DisplayName = "Alert Dialog Header",
        Description = "Header subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogHeader.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "header" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialogHeader.razor");
}
