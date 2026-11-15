using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AvatarVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "avatar-variants",
        DisplayName = "Avatar Variants",
        Description = "Variant definitions for Avatar component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Variants/AvatarVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Variants/AvatarVariants.cs");
}


