using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TextareaVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "textarea-variants",
        DisplayName = "Textarea Variants",
        Description = "Textarea utility classes (uses InputVariant)",
        Category = ComponentCategory.Utility,
        FilePath = "Variants/TextareaVariants.cs",
        IsAvailable = false,
        Dependencies = new List<string> { "input-variants" }
    };

    public static string Content => @"namespace YourProjectNamespace.Components.UI.Variants;

public static class TextareaVariants
{
    public static string Get(InputVariant variant = InputVariant.Default, string? className = null)
    {
        var baseClasses = ""flex min-h-[80px] w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"";

        var variantClasses = variant switch
        {
            InputVariant.Error => ""border-destructive focus-visible:ring-destructive"",
            _ => """"
        };

        return Shell.Cn(baseClasses, variantClasses, className);
    }
}
";
}
