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

    public static string Content => ComponentSource.Read("Components/Timeline.razor");
}
