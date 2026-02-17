using System.Collections.Generic;

namespace ShellUI.Components;

public enum SheetSide
{
    Top,
    Bottom,
    Left,
    Right
}

public static class SheetVariants
{
    private static readonly Dictionary<SheetSide, string> SideClasses = new()
    {
        { SheetSide.Top, "inset-x-0 top-0 border-b border-border/40 slide-in-from-top" },
        { SheetSide.Bottom, "inset-x-0 bottom-0 border-t border-border/40 slide-in-from-bottom" },
        { SheetSide.Left, "inset-y-0 left-0 h-full w-3/4 sm:max-w-sm border-r border-border/40 slide-in-from-left" },
        { SheetSide.Right, "inset-y-0 right-0 h-full w-3/4 sm:max-w-sm border-l border-border/40 slide-in-from-right" }
    };

    public static string Get(SheetSide side = SheetSide.Right, string? className = null)
    {
        var baseClasses = "fixed z-50 gap-4 bg-background p-6 shadow-lg transition ease-in-out animate-in duration-300 data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:duration-300 data-[state=open]:duration-500";
        return Shell.Cn(baseClasses, SideClasses.GetValueOrDefault(side, SideClasses[SheetSide.Right]), className);
    }
}

