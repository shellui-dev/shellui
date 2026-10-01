using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetCloseTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-close",
        DisplayName = "Sheet Close",
        Description = "Close button subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetClose.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "close" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Close"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] private Sheet? Parent { get; set; }
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
