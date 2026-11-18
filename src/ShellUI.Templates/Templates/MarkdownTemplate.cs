using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class MarkdownTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "markdown",
        DisplayName = "Markdown",
        Description = "Renders Markdown, such as a model's reply, safely and in the theme's styles",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Markdown.razor",
        Dependencies = new List<string> { "code-block" },
        NuGetDependencies = new List<NuGetDependency>
        {
            new() { PackageId = "Markdig", Version = "1.6.0" }
        },
        Tags = new List<string> { "markdown", "chat", "ai", "content", "prose" }
    };

    public static string Content => ComponentSource.Read("Components/Markdown.razor");
}
