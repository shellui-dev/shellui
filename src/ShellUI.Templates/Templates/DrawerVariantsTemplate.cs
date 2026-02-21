using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerVariantsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-variants",
        DisplayName = "Drawer Variants",
        Description = "DrawerSide enum and DrawerVariants utility class for Drawer subcomponents",
        Category = ComponentCategory.Utility,
        FilePath = "Variants/DrawerVariants.cs",
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "drawer", "variants" }
    };

    public static string Content => @"using System.Collections.Generic;

namespace YourProjectNamespace.Components.UI.Variants;

public enum DrawerSide
{
    Top,
    Bottom,
    Left,
    Right
}

public static class DrawerVariants
{
    private static readonly Dictionary<DrawerSide, string> SideClasses = new()
    {
        { DrawerSide.Top, ""left-1/2 top-0 -translate-x-1/2 mb-24 w-full max-w-md mx-4 flex h-auto flex-col rounded-b-[10px] bg-background shadow-lg slide-in-from-top"" },
        { DrawerSide.Bottom, ""left-1/2 bottom-0 -translate-x-1/2 mt-24 w-full max-w-md mx-4 flex h-auto flex-col rounded-t-[10px] bg-background shadow-lg slide-in-from-bottom"" },
        { DrawerSide.Left, ""inset-y-0 left-0 h-full w-3/4 sm:max-w-sm slide-in-from-left bg-background shadow-lg"" },
        { DrawerSide.Right, ""inset-y-0 right-0 h-full w-3/4 sm:max-w-sm slide-in-from-right bg-background shadow-lg"" }
    };

    public static string Get(DrawerSide side = DrawerSide.Bottom, string? className = null)
    {
        var baseClasses = ""fixed z-50 gap-4 bg-background p-6 shadow-lg transition ease-in-out animate-in"";
        return Shell.Cn(baseClasses, SideClasses.GetValueOrDefault(side, SideClasses[DrawerSide.Bottom]), className);
    }
}
";
}
