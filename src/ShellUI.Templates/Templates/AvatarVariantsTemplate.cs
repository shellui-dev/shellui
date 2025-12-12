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
        Version = "0.1.0",
        Dependencies = new List<string>()
    };

    public static string Content => @"using System.Collections.Generic;

namespace YourProjectNamespace.Components.UI;

public enum AvatarSize
{
    Default,
    Sm,
    Lg,
    Xl
}

public static class AvatarVariants
{
    private static readonly Dictionary<AvatarSize, string> SizeClasses = new()
    {
        { AvatarSize.Default, ""h-10 w-10"" },
        { AvatarSize.Sm, ""h-8 w-8"" },
        { AvatarSize.Lg, ""h-12 w-12"" },
        { AvatarSize.Xl, ""h-16 w-16"" }
    };

    public static string Get(AvatarSize size = AvatarSize.Default, string? className = null)
    {
        var baseClasses = ""relative flex shrink-0 overflow-hidden rounded-full"";
        
        return Shell.Cn(baseClasses, SizeClasses.GetValueOrDefault(size, SizeClasses[AvatarSize.Default]), className);
    }
}";
}

