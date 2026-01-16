using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AlertTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "alert",
        DisplayName = "Alert",
        Description = "Notification and status message component",
        Category = ComponentCategory.Feedback,

        FilePath = "Alert.razor",
        Dependencies = new List<string> { "alert-variants" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants

<div role=""alert"" class=""@ComputedClass"" @attributes=""AdditionalAttributes"">
    @if (Icon != null)
    {
        <div class=""flex-shrink-0"">
            @Icon
        </div>
    }
    <div class=""flex-1"">
        @if (!string.IsNullOrEmpty(Title))
        {
            <h5 class=""mb-1 font-medium leading-none tracking-tight"">@Title</h5>
        }
        <div class=""text-sm opacity-90"">
            @ChildContent
        </div>
    </div>
</div>

@code {
    [Parameter] public AlertVariant Variant { get; set; } = AlertVariant.Default;
    [Parameter] public string Title { get; set; } = """";
    [Parameter] public RenderFragment? Icon { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string ComputedClass => AlertVariants.Get(Variant, Class);
}
";
}

