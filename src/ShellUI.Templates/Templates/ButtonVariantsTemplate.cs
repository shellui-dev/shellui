using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ButtonVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "button-variants",
        DisplayName = "Button Variants",
        Description = "Button variant enums and utility classes",
        Category = ComponentCategory.Utility,

        FilePath = "Variants/ButtonVariants.cs",
        IsAvailable = false
    };

    public static string Content => @"using System.Collections.Generic;

namespace YourProjectNamespace.Components.UI.Variants;

public enum ButtonVariant
{
    Default,
    Destructive,
    Outline,
    Secondary,
    Ghost,
    Link
}

public enum ButtonSize
{
    Default,
    Sm,
    Lg,
    Icon
}

public static class ButtonVariants
{
    private static readonly Dictionary<ButtonVariant, string> VariantClasses = new()
    {
        { ButtonVariant.Default, ""bg-primary text-primary-foreground hover:bg-primary/90"" },
        { ButtonVariant.Destructive, ""bg-destructive text-destructive-foreground hover:bg-destructive/90"" },
        { ButtonVariant.Outline, ""border border-input bg-background hover:bg-accent hover:text-accent-foreground"" },
        { ButtonVariant.Secondary, ""bg-secondary text-secondary-foreground hover:bg-secondary/80"" },
        { ButtonVariant.Ghost, ""hover:bg-accent hover:text-accent-foreground"" },
        { ButtonVariant.Link, ""text-primary underline-offset-4 hover:underline"" }
    };

    private static readonly Dictionary<ButtonSize, string> SizeClasses = new()
    {
        { ButtonSize.Default, ""h-10 px-4 py-2"" },
        { ButtonSize.Sm, ""h-9 rounded-md px-3"" },
        { ButtonSize.Lg, ""h-11 rounded-md px-8"" },
        { ButtonSize.Icon, ""h-10 w-10"" }
    };

    public static string Get(ButtonVariant variant = ButtonVariant.Default,
                             ButtonSize size = ButtonSize.Default,
                             string? className = null)
    {
        var baseClasses = ""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"";

        return Shell.Cn(
            baseClasses,
            VariantClasses.GetValueOrDefault(variant, VariantClasses[ButtonVariant.Default]),
            SizeClasses.GetValueOrDefault(size, SizeClasses[ButtonSize.Default]),
            className
        );
    }
}
";
}

