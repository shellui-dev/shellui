using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DatePickerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "date-picker",
        DisplayName = "Date Picker",
        Description = "Calendar date picker component",
        Category = ComponentCategory.Form,
        FilePath = "DatePicker.razor",

        Tags = new List<string> { "form", "date", "calendar", "input" }
    };

    public static string Content => ComponentSource.Read("Components/DatePicker.razor");
}


