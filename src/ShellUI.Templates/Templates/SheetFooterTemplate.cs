using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-footer",
        DisplayName = "Sheet Footer",
        Description = "Footer subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetFooter.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "footer" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""flex flex-col-reverse gap-2 sm:flex-row sm:items-center sm:justify-end"", Class)"" @attributes=""AdditionalAttributes"">
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
