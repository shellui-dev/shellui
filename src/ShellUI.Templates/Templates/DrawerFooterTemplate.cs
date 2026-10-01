using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-footer",
        DisplayName = "Drawer Footer",
        Description = "Footer subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerFooter.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "drawer", "footer" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""mt-auto flex flex-col gap-2 p-4"", Class)"" @attributes=""AdditionalAttributes"">
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
