using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DateRangePickerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "date-range-picker",
        DisplayName = "Date Range Picker",
        Description = "Date range picker component for selecting start and end dates",
        Category = ComponentCategory.Form,
        FilePath = "DateRangePicker.razor",

        Tags = new List<string> { "form", "date", "range", "calendar", "input" }
    };

    public static string Content => ComponentSource.Read("Components/DateRangePicker.razor");
}

