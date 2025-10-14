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

<div role=""alert"" class=""@CssClass"" @attributes=""AdditionalAttributes"">
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
    [Parameter] public string Variant { get; set; } = ""default"";
    [Parameter] public string Title { get; set; } = """";
    [Parameter] public RenderFragment? Icon { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string CssClass => BuildCssClass();

    private string BuildCssClass()
    {
        var classes = new List<string>
        {
            ""relative w-full rounded-lg border p-4 flex gap-3"",
            ""[&>svg~*]:pl-7 [&>svg+div]:translate-y-[-3px]"",
            ""[&>svg]:absolute [&>svg]:left-4 [&>svg]:top-4 [&>svg]:text-foreground""
        };

        // Add variant-specific styling
        switch (Variant.ToLower())
        {
            case ""destructive"":
                classes.Add(""border-destructive/50 text-destructive dark:border-destructive [&>svg]:text-destructive"");
                break;
            case ""success"":
                classes.Add(""border-green-500/50 text-green-700 dark:border-green-500 dark:text-green-400 [&>svg]:text-green-600"");
                break;
            case ""warning"":
                classes.Add(""border-yellow-500/50 text-yellow-700 dark:border-yellow-500 dark:text-yellow-400 [&>svg]:text-yellow-600"");
                break;
            case ""info"":
                classes.Add(""border-blue-500/50 text-blue-700 dark:border-blue-500 dark:text-blue-400 [&>svg]:text-blue-600"");
                break;
            default:
                classes.Add(""bg-background text-foreground"");
                break;
        }

        return string.Join("" "", classes);
    }
}
";
}

