using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-trigger",
        DisplayName = "Alert Dialog Trigger",
        Description = "Trigger subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "trigger" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialogTrigger.razor");
}
