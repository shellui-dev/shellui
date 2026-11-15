using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-title",
        DisplayName = "Alert Dialog Title",
        Description = "Title subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogTitle.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "title" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialogTitle.razor");
}
