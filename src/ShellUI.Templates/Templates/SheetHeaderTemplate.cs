using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-header",
        DisplayName = "Sheet Header",
        Description = "Header subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetHeader.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "header" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""flex flex-col space-y-2 text-center sm:text-left"", Class)"" @attributes=""AdditionalAttributes"">
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
