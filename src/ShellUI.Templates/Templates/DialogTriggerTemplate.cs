using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-trigger",
        DisplayName = "Dialog Trigger",
        Description = "Trigger button for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogTrigger.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/DialogTrigger.razor");
}


