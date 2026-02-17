using System.Collections.Generic;

namespace ShellUI.Components;

public enum AlertVariant
{
    Default,
    Destructive,
    Success,
    Warning,
    Info
}

public static class AlertVariants
{
    private static readonly Dictionary<AlertVariant, string> VariantClasses = new()
    {
        { AlertVariant.Default, "border-border bg-background text-foreground" },
        { AlertVariant.Destructive, "border-border text-destructive [&>svg]:text-destructive" },
        { AlertVariant.Success, "border-border text-green-700 dark:text-green-400 [&>svg]:text-green-600" },
        { AlertVariant.Warning, "border-border text-yellow-700 dark:text-yellow-400 [&>svg]:text-yellow-600" },
        { AlertVariant.Info, "border-border text-blue-700 dark:text-blue-400 [&>svg]:text-blue-600" }
    };

    public static string Get(AlertVariant variant = AlertVariant.Default, string? className = null)
    {
        var baseClasses = "relative w-full rounded-lg border p-4 flex gap-3 [&>svg~*]:pl-7 [&>svg+div]:translate-y-[-3px] [&>svg]:absolute [&>svg]:left-4 [&>svg]:top-4 [&>svg]:text-foreground";
        
        return Shell.Cn(baseClasses, VariantClasses.GetValueOrDefault(variant, VariantClasses[AlertVariant.Default]), className);
    }
}

