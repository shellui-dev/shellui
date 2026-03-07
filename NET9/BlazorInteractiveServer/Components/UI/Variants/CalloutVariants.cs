using System.Collections.Generic;

namespace BlazorInteractiveServer.Components.UI.Variants;

public enum CalloutVariant
{
    Info,
    Warning,
    Danger,
    Tip,
    Default
}

public static class CalloutVariants
{
    private static readonly Dictionary<CalloutVariant, string> VariantClasses = new()
    {
        { CalloutVariant.Info, "border-border bg-blue-500/5 text-blue-700 dark:bg-blue-500/10 dark:text-blue-400 [&>svg]:text-blue-600 dark:[&>svg]:text-blue-400" },
        { CalloutVariant.Warning, "border-border bg-amber-500/5 text-amber-700 dark:bg-amber-500/10 dark:text-amber-400 [&>svg]:text-amber-600 dark:[&>svg]:text-amber-400" },
        { CalloutVariant.Danger, "border-border bg-red-500/5 text-red-700 dark:bg-red-500/10 dark:text-red-400 [&>svg]:text-red-600 dark:[&>svg]:text-red-400" },
        { CalloutVariant.Tip, "border-border bg-emerald-500/5 text-emerald-700 dark:bg-emerald-500/10 dark:text-emerald-400 [&>svg]:text-emerald-600 dark:[&>svg]:text-emerald-400" },
        { CalloutVariant.Default, "border-border bg-muted/50 text-muted-foreground [&>svg]:text-muted-foreground" }
    };

    public static string Get(CalloutVariant variant = CalloutVariant.Info, string? className = null)
    {
        var baseClasses = "rounded-lg border px-4 py-3 my-6 [&>svg]:inline-block [&>svg]:size-4 [&>svg]:shrink-0";
        return Shell.Cn(baseClasses, VariantClasses.GetValueOrDefault(variant, VariantClasses[CalloutVariant.Info]), className);
    }
}
