using System.Collections.Generic;

namespace ShellUI.Components;

public enum InputVariant
{
    Default,
    Error
}

public static class InputVariants
{
    public static string Get(InputVariant variant = InputVariant.Default, string? className = null)
    {
        var baseClasses = "flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50";
        
        var variantClasses = variant switch
        {
            InputVariant.Error => "border-destructive focus-visible:ring-destructive",
            _ => ""
        };

        return Shell.Cn(baseClasses, variantClasses, className);
    }
}

