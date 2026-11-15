using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AvatarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "avatar",
        DisplayName = "Avatar",
        Description = "User avatar image component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Avatar.razor",

        Dependencies = new List<string> { "avatar-variants" },
        Tags = new List<string> { "avatar", "image", "user", "profile" }
    };

    public static string Content => ComponentSource.Read("Components/Avatar.razor");
}

