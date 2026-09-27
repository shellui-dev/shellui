using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TimelineTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "timeline",
        DisplayName = "Timeline",
        Description = "Vertical timeline of events",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Timeline.razor",
        Dependencies = new List<string> { "timeline-item" },
        Tags = new List<string> { "timeline", "history", "activity" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<ol class=""@Shell.Cn(""relative"", Class)"" @attributes=""AdditionalAttributes"">
    @ChildContent
</ol>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
