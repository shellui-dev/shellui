using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CalloutTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "callout",
        DisplayName = "Callout",
        Description = "Info/warning/danger/tip callout boxes for documentation",
        Category = ComponentCategory.Feedback,
        FilePath = "Callout.razor",
        Dependencies = new List<string> { "callout-variants" },
        Variants = new List<string> { "info", "warning", "danger", "tip", "default" },
        Tags = new List<string> { "docs", "callout", "admonition", "info", "warning", "tip" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants

<div role=""status"" class=""@ComputedClass"" @attributes=""AdditionalAttributes"">
    <div class=""flex gap-3"">
        @if (Icon != null)
        {
            <span class=""flex-shrink-0 mt-0.5"">@Icon</span>
        }
        <div class=""flex-1 min-w-0"">
            @if (!string.IsNullOrEmpty(Title))
            {
                <div class=""mb-2 font-semibold leading-none tracking-tight"">@Title</div>
            }
            <div class=""text-sm [&>p]:leading-7 [&>p:first-child]:mt-0 [&>p:last-child]:mb-0"">
                @ChildContent
            </div>
        </div>
    </div>
</div>

@code {
    [Parameter] public CalloutVariant Variant { get; set; } = CalloutVariant.Info;
    [Parameter] public string? Title { get; set; }
    [Parameter] public RenderFragment? Icon { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string ComputedClass => CalloutVariants.Get(Variant, Class);
}
";
}
