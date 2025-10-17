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
        Version = "0.1.0",
        FilePath = "Alert.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""relative w-full rounded-lg border border-border p-4 @(Variant == ""destructive"" ? ""border-destructive/50 text-destructive dark:border-destructive [&>svg]:text-destructive"" : """")"">
    @if (!string.IsNullOrEmpty(Icon))
    {
        <div class=""flex items-start gap-4"">
            <div class=""[&>svg]:absolute [&>svg]:left-4 [&>svg]:top-4 [&>svg]:text-foreground"">
                @((MarkupString)Icon)
            </div>
            <div class=""flex-1 [&_p]:leading-relaxed"">
                @if (!string.IsNullOrEmpty(Title))
                {
                    <h5 class=""mb-1 font-medium leading-none tracking-tight"">@Title</h5>
                }
                @ChildContent
            </div>
        </div>
    }
    else
    {
        @if (!string.IsNullOrEmpty(Title))
        {
            <h5 class=""mb-1 font-medium leading-none tracking-tight"">@Title</h5>
        }
        <div class=""text-sm [&_p]:leading-relaxed"">
            @ChildContent
        </div>
    }
</div>

@code {
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string Variant { get; set; } = ""default"";
}
";
}

