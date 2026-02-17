using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class PopoverContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "popover-content",
        DisplayName = "Popover Content",
        Description = "Content panel for Popover (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "PopoverContent.razor",
        Dependencies = new List<string> { "popover" },
        IsAvailable = false,
        Tags = new List<string> { "overlay", "popover", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (Parent?.IsOpen == true)
{
    <div class=""@Shell.Cn(""absolute z-50 w-72 rounded-md border border-border bg-popover p-4 text-popover-foreground shadow-md outline-none animate-in fade-in-0 zoom-in-95"", Parent.PlacementClass)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] public Popover? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
