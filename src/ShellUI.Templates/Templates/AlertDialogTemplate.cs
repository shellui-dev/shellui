using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AlertDialogTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog",
        DisplayName = "Alert Dialog",
        Description = "Modal dialog for confirmations and alerts",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialog.razor",
        Dependencies = new List<string> { "dialog", "button", "alert-dialog-trigger", "alert-dialog-content", "alert-dialog-header", "alert-dialog-title", "alert-dialog-description", "alert-dialog-footer", "alert-dialog-action", "alert-dialog-cancel" }
    };

    public static string Content => ComponentSource.Read("Components/AlertDialog.razor", variantsNamespace: true);
}

