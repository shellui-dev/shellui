using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CardFooterTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "card-footer",
        DisplayName = "Card Footer",
        Description = "Footer for the Card component",
        Category = ComponentCategory.Layout,
        Version = "0.1.0",
        FilePath = "CardFooter.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace ShellUI.Components

<div class=""@Shell.Cn(""flex items-center p-6 pt-0"", Class)"" @attributes=""AdditionalAttributes"">
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

