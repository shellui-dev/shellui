using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ContextMenuTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "context-menu-trigger",
        DisplayName = "Context Menu Trigger",
        Description = "Right-click trigger area for ContextMenu",
        Category = ComponentCategory.Overlay,
        FilePath = "ContextMenuTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "context-menu", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] public ContextMenu? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
