using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class KbdTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "kbd",
        DisplayName = "Kbd",
        Description = "Keyboard key hint for shortcuts",
        Category = ComponentCategory.Typography,
        FilePath = "Kbd.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "kbd", "keyboard", "shortcut" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<kbd class=""@Shell.Cn(""pointer-events-none inline-flex h-5 min-w-5 select-none items-center justify-center gap-1 rounded border border-border bg-muted px-1.5 font-mono text-[11px] font-medium text-muted-foreground"", Class)"" @attributes=""AdditionalAttributes"">@ChildContent</kbd>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
