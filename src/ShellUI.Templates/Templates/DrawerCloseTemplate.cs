using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerCloseTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-close",
        DisplayName = "Drawer Close",
        Description = "Close button subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerClose.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "close" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Close"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] private Drawer? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Close()
    {
        if (Parent != null) await Parent.SetOpen(false);
    }
}
";
}
