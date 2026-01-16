using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardHeaderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-header",
        DisplayName = "Card Header",
        Description = "Header for the Card component",
        Category = ComponentCategory.Layout,

        FilePath = "CardHeader.razor",
        IsAvailable = false // Hidden from list, installed via dependency
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""flex flex-col space-y-1.5 p-6"", Class)"" @attributes=""AdditionalAttributes"">
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


