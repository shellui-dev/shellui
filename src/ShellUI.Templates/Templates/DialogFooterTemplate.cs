using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-footer",
        DisplayName = "Dialog Footer",
        Description = "Footer for Dialog",
        Category = ComponentCategory.Overlay,
        Version = "0.1.0",
        FilePath = "DialogFooter.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""flex flex-col-reverse sm:flex-row sm:justify-end sm:space-x-2"", Class)"" @attributes=""AdditionalAttributes"">
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

