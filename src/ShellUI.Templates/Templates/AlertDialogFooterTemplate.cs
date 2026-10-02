using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-footer",
        DisplayName = "Alert Dialog Footer",
        Description = "Footer subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogFooter.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "footer" }
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
