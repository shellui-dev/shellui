using System.Collections.Generic;

namespace ShellUI.Components;

public enum SonnerPosition
{
    TopLeft,
    TopCenter,
    TopRight,
    BottomLeft,
    BottomCenter,
    BottomRight
}

public static class SonnerVariants
{
    private static readonly Dictionary<SonnerPosition, (string Container, string Animation)> PositionClasses = new()
    {
        { SonnerPosition.TopLeft, ("top-0 left-0 p-4", "slide-in-from-top-2") },
        { SonnerPosition.TopCenter, ("top-0 left-1/2 -translate-x-1/2 p-4", "slide-in-from-top-2") },
        { SonnerPosition.TopRight, ("top-0 right-0 p-4", "slide-in-from-top-2") },
        { SonnerPosition.BottomLeft, ("bottom-0 left-0 p-4", "slide-in-from-bottom-2") },
        { SonnerPosition.BottomCenter, ("bottom-0 left-1/2 -translate-x-1/2 p-4", "slide-in-from-bottom-2") },
        { SonnerPosition.BottomRight, ("bottom-0 right-0 p-4", "slide-in-from-bottom-2") }
    };

    public static string GetContainerClasses(SonnerPosition position)
    {
        var (container, _) = PositionClasses.GetValueOrDefault(position, PositionClasses[SonnerPosition.TopCenter]);
        var isTop = position is SonnerPosition.TopLeft or SonnerPosition.TopCenter or SonnerPosition.TopRight;
        var flexDir = isTop ? "flex-col" : "flex-col-reverse";
        return $"fixed z-50 group {container} flex {flexDir} max-h-screen w-[380px] gap-2 hover:gap-4 overflow-x-hidden overflow-y-auto transition-all duration-300 ease-out";
    }

    public static string GetToastAnimation(SonnerPosition position)
    {
        var (_, animation) = PositionClasses.GetValueOrDefault(position, PositionClasses[SonnerPosition.TopCenter]);
        return animation;
    }
}
