using System.Collections.Generic;

namespace BlazorInteractiveServer.Components;

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
        { DrawerSide.Top, "inset-x-0 top-0 mb-24 flex h-auto flex-col rounded-b-[10px] border bg-background slide-in-from-top" },
        { DrawerSide.Bottom, "inset-x-0 bottom-0 mt-24 flex h-auto flex-col rounded-t-[10px] border bg-background slide-in-from-bottom" },
        { DrawerSide.Left, "inset-y-0 left-0 h-full w-3/4 sm:max-w-sm border-r slide-in-from-left bg-background" },
        { DrawerSide.Right, "inset-y-0 right-0 h-full w-3/4 sm:max-w-sm border-l slide-in-from-right bg-background" }
    };

    public static string Get(DrawerSide side = DrawerSide.Bottom, string? className = null)
    {
        var baseClasses = "fixed z-50 gap-4 bg-background p-6 shadow-lg transition ease-in-out animate-in";
        return Shell.Cn(baseClasses, SideClasses.GetValueOrDefault(side, SideClasses[DrawerSide.Bottom]), className);
    }
}


