using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-header",
        DisplayName = "Dialog Header",
        Description = "Header for Dialog",
        Category = ComponentCategory.Overlay,

        FilePath = "DialogHeader.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""flex flex-col space-y-1.5 text-center sm:text-left"", Class)"" @attributes=""AdditionalAttributes"">
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


