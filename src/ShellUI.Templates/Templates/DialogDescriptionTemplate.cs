using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-description",
        DisplayName = "Dialog Description",
        Description = "Description for Dialog",
        Category = ComponentCategory.Overlay,
        Version = "0.1.0",
        FilePath = "DialogDescription.razor",
        IsAvailable = false
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

