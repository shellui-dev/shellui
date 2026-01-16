using System.Linq;

namespace BlazorInteractiveServer.Components;

public static class Shell
{
    public static string Cn(params string?[] classes)
        => string.Join(" ", classes.Where(c => !string.IsNullOrWhiteSpace(c)));
}

