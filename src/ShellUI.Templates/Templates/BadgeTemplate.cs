using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class BadgeTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "badge",
        DisplayName = "Badge",
        Description = "Small status indicator component",
        Category = ComponentCategory.DataDisplay,

        FilePath = "Badge.razor",
        Dependencies = new List<string> { "badge-variants" }
    };

    public static string Content => ComponentSource.Read("Components/Badge.razor");
}

