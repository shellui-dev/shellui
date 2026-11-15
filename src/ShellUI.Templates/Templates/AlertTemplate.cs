using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AlertTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert",
        DisplayName = "Alert",
        Description = "Notification and status message component",
        Category = ComponentCategory.Feedback,

        FilePath = "Alert.razor",
        Dependencies = new List<string> { "alert-variants" }
    };

    public static string Content => ComponentSource.Read("Components/Alert.razor");
}

