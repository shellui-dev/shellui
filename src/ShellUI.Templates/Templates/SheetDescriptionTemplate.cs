using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-description",
        DisplayName = "Sheet Description",
        Description = "Description subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetDescription.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "sheet", "description" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<p class=""@Shell.Cn(""text-sm text-muted-foreground"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</p>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
