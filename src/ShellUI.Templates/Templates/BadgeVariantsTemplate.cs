using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class BadgeVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "badge-variants",
        DisplayName = "Badge Variants",
        Description = "Variant definitions for Badge component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Variants/BadgeVariants.cs",
        Version = "0.1.0",
        Dependencies = new List<string>()
    };

    public static string Content => @"using System.Collections.Generic;

namespace YourProjectNamespace.Components.UI;

public enum BadgeVariant
{
    Default,
    Secondary,
    Destructive,
    Outline,
    Success,
    Warning,
    Info
}

public static class BadgeVariants
{
    private static readonly Dictionary<BadgeVariant, string> VariantClasses = new()
    {
        { BadgeVariant.Default, ""border-transparent bg-primary text-primary-foreground hover:bg-primary/80"" },
        { BadgeVariant.Secondary, ""border-transparent bg-secondary text-secondary-foreground hover:bg-secondary/80"" },
        { BadgeVariant.Destructive, ""border-transparent bg-destructive text-destructive-foreground hover:bg-destructive/80"" },
        { BadgeVariant.Outline, ""text-foreground"" },
        { BadgeVariant.Success, ""border-transparent bg-green-500 text-white hover:bg-green-600"" },
        { BadgeVariant.Warning, ""border-transparent bg-yellow-500 text-white hover:bg-yellow-600"" },
        { BadgeVariant.Info, ""border-transparent bg-blue-500 text-white hover:bg-blue-600"" }
    };

    public static string Get(BadgeVariant variant = BadgeVariant.Default, string? className = null)
    {
        var baseClasses = ""inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold transition-colors focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"";
        
        return Shell.Cn(
            baseClasses,
            VariantClasses.GetValueOrDefault(variant, VariantClasses[BadgeVariant.Default]),
            className
        );
    }
}";
}

