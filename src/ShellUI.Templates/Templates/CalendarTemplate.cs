using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CalendarTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "calendar",
        DisplayName = "Calendar",
        Description = "Full calendar component for date selection",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Calendar.razor",
        Dependencies = new List<string>()
    };

    public static string Content => ComponentSource.Read("Components/Calendar.razor");
}

