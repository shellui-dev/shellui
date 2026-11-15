using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TimePickerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "time-picker",
        DisplayName = "Time Picker",
        Description = "Time picker component with hour and minute selection",
        Category = ComponentCategory.Form,
        FilePath = "TimePicker.razor",

        Tags = new List<string> { "form", "time", "input", "clock" }
    };

    public static string Content => ComponentSource.Read("Components/TimePicker.razor");
}


