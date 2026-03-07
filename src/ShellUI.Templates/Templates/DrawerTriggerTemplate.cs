using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-trigger",
        DisplayName = "Drawer Trigger",
        Description = "Trigger subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerTrigger.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "drawer", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Open"" role=""button"" tabindex=""0"" @onkeydown=""HandleKeyDown"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] private Drawer? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Open()
    {
        if (Parent != null) await Parent.SetOpen(true);
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if ((e.Key == ""Enter"" || e.Key == "" "") && Parent != null) await Parent.SetOpen(true);
    }
}
";
}
