using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogDescriptionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-description",
        DisplayName = "Alert Dialog Description",
        Description = "Description subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogDescription.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "description" }
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
