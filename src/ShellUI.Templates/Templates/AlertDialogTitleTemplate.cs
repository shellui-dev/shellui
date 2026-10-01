using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AlertDialogTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert-dialog-title",
        DisplayName = "Alert Dialog Title",
        Description = "Title subcomponent for the compositional AlertDialog pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "AlertDialogTitle.razor",
        IsAvailable = false,
        Tags = new List<string> { "overlay", "alert-dialog", "title" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<h2 class=""@Shell.Cn(""text-lg font-semibold"", Class)"" @attributes=""AdditionalAttributes"">
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
