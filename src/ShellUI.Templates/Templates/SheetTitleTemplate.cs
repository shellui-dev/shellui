using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-title",
        DisplayName = "Sheet Title",
        Description = "Title subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetTitle.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "title" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<h2 class=""@Shell.Cn(""text-lg font-semibold text-foreground"", Class)"" @attributes=""AdditionalAttributes"">
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
