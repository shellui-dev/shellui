using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class StatCardTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "stat-card",
        DisplayName = "Stat Card",
        Description = "KPI tile with value, change indicator and description",
        Category = ComponentCategory.DataDisplay,
        FilePath = "StatCard.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "stat", "kpi", "metric", "dashboard" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(""rounded-lg border border-border bg-card p-6 text-card-foreground shadow-sm"", Class)"" @attributes=""AdditionalAttributes"">
    <div class=""flex items-center justify-between gap-2"">
        <p class=""text-sm font-medium text-muted-foreground"">@Title</p>
        @if (Icon != null)
        {
            <span class=""flex text-muted-foreground [&>svg]:size-4"">@Icon</span>
        }
    </div>
    <div class=""mt-2 flex flex-wrap items-baseline gap-2"">
        <p class=""text-2xl font-bold tracking-tight tabular-nums"">@Value</p>
        @if (!string.IsNullOrEmpty(Change))
        {
            <span class=""@Shell.Cn(""inline-flex items-center rounded-md px-1.5 py-0.5 text-xs font-medium"", ChangeClass)"">@Change</span>
        }
    </div>
    @if (!string.IsNullOrEmpty(Description))
    {
        <p class=""mt-1 text-xs text-muted-foreground"">@Description</p>
    }
    @ChildContent
</div>

@code {
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Value { get; set; }
    [Parameter] public string? Change { get; set; }
    [Parameter] public string Trend { get; set; } = ""neutral"";
    [Parameter] public string? Description { get; set; }
    [Parameter] public RenderFragment? Icon { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string ChangeClass => Trend.ToLowerInvariant() switch
    {
        ""up"" => ""bg-emerald-500/10 text-emerald-600 dark:text-emerald-400"",
        ""down"" => ""bg-red-500/10 text-red-600 dark:text-red-400"",
        _ => ""bg-muted text-muted-foreground""
    };
}
";
}
