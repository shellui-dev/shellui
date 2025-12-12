using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DialogTitleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "dialog-title",
        DisplayName = "Dialog Title",
        Description = "Title for Dialog",
        Category = ComponentCategory.Overlay,
        Version = "0.1.0",
        FilePath = "DialogTitle.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<h2 class=""@Shell.Cn(""text-lg font-semibold leading-none tracking-tight"", Class)"" @attributes=""AdditionalAttributes"">
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

