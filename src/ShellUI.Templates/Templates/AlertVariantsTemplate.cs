using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AlertVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-variants",
        DisplayName = "Alert Variants",
        Description = "Variant definitions for Alert component",
        Category = ComponentCategory.Feedback,
        FilePath = "Variants/AlertVariants.cs",

        Dependencies = new List<string>()
    };

    public static string Content => @"using System.Collections.Generic;

namespace YourProjectNamespace.Components.UI;

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
        { AlertVariant.Default, ""bg-background text-foreground"" },
        { AlertVariant.Destructive, ""border-destructive/50 text-destructive dark:border-destructive [&>svg]:text-destructive"" },
        { AlertVariant.Success, ""border-green-500/50 text-green-700 dark:border-green-500 dark:text-green-400 [&>svg]:text-green-600"" },
        { AlertVariant.Warning, ""border-yellow-500/50 text-yellow-700 dark:border-yellow-500 dark:text-yellow-400 [&>svg]:text-yellow-600"" },
        { AlertVariant.Info, ""border-blue-500/50 text-blue-700 dark:border-blue-500 dark:text-blue-400 [&>svg]:text-blue-600"" }
    };

    public static string Get(AlertVariant variant = AlertVariant.Default, string? className = null)
    {
        var baseClasses = ""relative w-full rounded-lg border p-4 flex gap-3 [&>svg~*]:pl-7 [&>svg+div]:translate-y-[-3px] [&>svg]:absolute [&>svg]:left-4 [&>svg]:top-4 [&>svg]:text-foreground"";
        
        return Shell.Cn(baseClasses, VariantClasses.GetValueOrDefault(variant, VariantClasses[AlertVariant.Default]), className);
    }
}";
}


