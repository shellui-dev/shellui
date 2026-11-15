using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SkeletonTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "skeleton",
        DisplayName = "Skeleton",
        Description = "Loading placeholder with pulse animation",
        Category = ComponentCategory.Feedback,

        FilePath = "Skeleton.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Skeleton.razor");
}


