using System.Collections.Generic;

namespace BlazorInteractiveServer.Components;

public enum ToggleVariant
{
    Default,
    Outline
}

public enum ToggleSize
{
    Default,
    Sm,
    Lg
}

public static class ToggleVariants
{
    private static readonly Dictionary<ToggleVariant, Func<bool, string>> VariantClasses = new()
    {
        { ToggleVariant.Default, (pressed) => pressed ? "bg-accent text-accent-foreground" : "bg-transparent hover:bg-muted hover:text-muted-foreground" },
        { ToggleVariant.Outline, (pressed) => pressed ? "bg-accent" : "border border-input bg-transparent hover:bg-accent hover:text-accent-foreground" }
    };

    private static readonly Dictionary<ToggleSize, string> SizeClasses = new()
    {
        { ToggleSize.Default, "h-10 px-3" },
        { ToggleSize.Sm, "h-9 px-2.5" },
        { ToggleSize.Lg, "h-11 px-5" }
    };

    public static string Get(ToggleVariant variant = ToggleVariant.Default, 
                             ToggleSize size = ToggleSize.Default, 
                             bool pressed = false,
                             string? className = null)
    {
        var baseClasses = "inline-flex items-center justify-center rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50";
        
        return Shell.Cn(
            baseClasses,
            VariantClasses.GetValueOrDefault(variant, VariantClasses[ToggleVariant.Default])(pressed),
            SizeClasses.GetValueOrDefault(size, SizeClasses[ToggleSize.Default]),
            className
        );
    }
}


