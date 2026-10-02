using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-header",
        DisplayName = "Drawer Header",
        Description = "Header subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerHeader.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "header" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""grid gap-1.5 p-4 text-center sm:text-left"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
