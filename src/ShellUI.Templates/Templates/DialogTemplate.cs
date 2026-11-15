using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog",
        DisplayName = "Dialog",
        Description = "Modal dialog component with composable parts",
        Category = ComponentCategory.Overlay,

        FilePath = "Dialog.razor",
        Dependencies = new List<string> { "dialog-trigger", "dialog-content", "dialog-header", "dialog-footer", "dialog-title", "dialog-description", "dialog-close" }
    };

    public static string Content => ComponentSource.Read("Components/Dialog.razor");
}

