using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ContextMenuContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "context-menu-content",
        DisplayName = "Context Menu Content",
        Description = "Menu panel for ContextMenu",
        Category = ComponentCategory.Overlay,
        FilePath = "ContextMenuContent.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "context-menu", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (Parent?.IsOpenState == true)
{
    <div class=""absolute right-0 z-50 mt-2 w-56 origin-top-right rounded-md border border-border bg-popover p-1 text-popover-foreground shadow-md"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] public ContextMenu? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
