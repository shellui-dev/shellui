using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-content",
        DisplayName = "Alert Dialog Content",
        Description = "Content subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "content" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialogContent.razor");
}
