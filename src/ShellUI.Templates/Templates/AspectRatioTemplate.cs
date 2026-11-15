using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AspectRatioTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "aspect-ratio",
        DisplayName = "Aspect Ratio",
        Description = "Container that keeps its content at a fixed aspect ratio",
        Category = ComponentCategory.Layout,
        FilePath = "AspectRatio.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "layout", "media", "ratio" }
    };

    public static string Content => ComponentSource.Read("Components/AspectRatio.razor");
}
