using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class LoadingTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "loading",
        DisplayName = "Loading",
        Description = "Loading spinner and skeleton components with multiple variants (requires CSS keyframes - see component comments)",
        Category = ComponentCategory.Feedback,
        FilePath = "Loading.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Loading.razor");
}

