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

    public static string Content => ComponentSource.Read("Components/TimelineItem.razor");
}
