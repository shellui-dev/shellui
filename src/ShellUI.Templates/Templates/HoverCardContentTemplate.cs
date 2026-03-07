using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class HoverCardContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "hover-card-content",
        DisplayName = "Hover Card Content",
        Description = "Content panel for HoverCard (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "HoverCardContent.razor",
        Dependencies = new List<string> { "hover-card" },
        IsAvailable = false,
        Tags = new List<string> { "overlay", "hover-card", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (Parent?.IsVisible == true)
{
    <div class=""@Shell.Cn(""absolute z-50 w-64 rounded-md border border-border bg-popover p-4 text-popover-foreground shadow-md outline-none animate-in fade-in-0 zoom-in-95"", Parent.PositionClasses)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] public HoverCard? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
