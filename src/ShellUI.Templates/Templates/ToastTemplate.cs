using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ToastTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "toast",
        DisplayName = "Toast",
        Description = "Toast notification component",
        Category = ComponentCategory.Feedback,
        FilePath = "Toast.razor",

        Variants = new List<string> { "default", "destructive", "success", "warning" },
        Tags = new List<string> { "notification", "toast", "feedback", "alert" }
    };

    public static string Content => ComponentSource.Read("Components/Toast.razor");
}


