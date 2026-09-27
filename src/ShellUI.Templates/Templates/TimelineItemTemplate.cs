using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TimelineItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "timeline-item",
        DisplayName = "Timeline Item",
        Description = "Event for Timeline",
        Category = ComponentCategory.DataDisplay,
        FilePath = "TimelineItem.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "timeline" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<li class=""@Shell.Cn(""group relative flex gap-3 pb-8 last:pb-0"", Class)"" @attributes=""AdditionalAttributes"">
    <span aria-hidden=""true"" class=""absolute bottom-0 left-3.5 top-0 w-px -translate-x-1/2 bg-border group-first:top-3.5 group-last:bottom-auto group-last:h-3.5 group-only:hidden""></span>
    <div class=""relative z-10 flex h-7 w-7 shrink-0 items-center justify-center"">
        @if (Icon != null)
        {
            <span class=""@Shell.Cn(""flex h-7 w-7 items-center justify-center rounded-full border bg-background [&>svg]:size-3.5"", Active ? ""border-primary text-primary"" : ""border-border text-muted-foreground"")"">@Icon</span>
        }
        else
        {
            <span class=""@Shell.Cn(""h-2.5 w-2.5 rounded-full"", Active ? ""bg-primary ring-4 ring-primary/20"" : ""bg-muted-foreground"")""></span>
        }
    </div>
    <div class=""min-w-0 flex-1"">
        <div class=""flex min-h-7 flex-wrap items-center gap-x-2"">
            <h3 class=""text-sm font-semibold text-foreground"">@Title</h3>
            @if (!string.IsNullOrEmpty(Time))
            {
                <time class=""text-xs text-muted-foreground"">@Time</time>
            }
        </div>
        @if (ChildContent != null)
        {
            <div class=""text-sm text-muted-foreground"">@ChildContent</div>
        }
    </div>
</li>

@code {
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Time { get; set; }
    [Parameter] public bool Active { get; set; }
    [Parameter] public RenderFragment? Icon { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
