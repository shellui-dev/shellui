using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-content",
        DisplayName = "Card Content",
        Description = "Content area for the Card component",
        Category = ComponentCategory.Layout,

        FilePath = "CardContent.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""p-6 pt-0"", Class)"" @attributes=""AdditionalAttributes"">
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


