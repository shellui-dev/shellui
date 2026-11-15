using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ProgressTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "progress",
        DisplayName = "Progress",
        Description = "Progress bar indicator with customizable height",
        Category = ComponentCategory.Feedback,

        FilePath = "Progress.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Progress.razor");
}


