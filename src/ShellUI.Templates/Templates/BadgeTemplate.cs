using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class BadgeTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "badge",
        DisplayName = "Badge",
        Description = "Small status indicator component",
        Category = ComponentCategory.DataDisplay,
        Version = "0.1.0",
        FilePath = "Badge.razor",
        Dependencies = new List<string> { "badge-variants" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants

<div class=""@ComputedClass"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [Parameter] public BadgeVariant Variant { get; set; } = BadgeVariant.Default;
    [Parameter] public string? Class { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string ComputedClass => BadgeVariants.Get(Variant, Class);
}
";
}
