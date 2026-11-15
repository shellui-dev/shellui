using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-description",
        DisplayName = "Alert Dialog Description",
        Description = "Description subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogDescription.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "description" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialogDescription.razor");
}
