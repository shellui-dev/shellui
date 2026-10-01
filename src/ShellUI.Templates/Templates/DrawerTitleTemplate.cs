using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-title",
        DisplayName = "Drawer Title",
        Description = "Title subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerTitle.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "title" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<h2 class=""@Shell.Cn(""text-lg font-semibold leading-none tracking-tight"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</h2>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
