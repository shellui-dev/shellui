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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@(""relative "" + ClassName)"" @attributes=""AdditionalAttributes"">
    <button type=""button""
            @onclick=""TogglePicker""
            disabled=""@Disabled""
            class=""@(""flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 "" + (Disabled ? ""opacity-50 cursor-not-allowed"" : """"))"">
        <span class=""@(SelectedTime == null ? ""text-muted-foreground"" : """")"">
            @(SelectedTime?.ToString(@""hh\:mm"") ?? Placeholder)
        </span>
        <svg class=""h-4 w-4 opacity-50"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"" />
        </svg>
    </button>
    
    @if (_isOpen)
    {
        <div class=""absolute z-50 mt-1 rounded-md border bg-popover p-4 text-popover-foreground shadow-md w-64"">
            <div class=""flex gap-2 mb-3"">
                <div class=""flex-1"">
                    <label class=""text-xs font-medium mb-1 block"">Hour</label>
                    <select @bind=""selectedHour"" class=""flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"">
                        @for (int h = 0; h < 24; h++)
                        {
                            <option value=""@h"">@h.ToString(""D2"")</option>
                        }
                    </select>
                </div>
                <div class=""flex-1"">
                    <label class=""text-xs font-medium mb-1 block"">Minute</label>
                    <select @bind=""selectedMinute"" class=""flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2"">
                        @for (int m = 0; m < 60; m++)
                        {
                            <option value=""@m"">@m.ToString(""D2"")</option>
                        }
                    </select>
                </div>
            </div>
            <button type=""button""
                    @onclick=""ApplyTime""
                    class=""w-full inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring h-9 px-4 py-2 bg-primary text-primary-foreground shadow hover:bg-primary/90"">
                Apply
            </button>
        </div>
    }
</div>

@code {
    [Parameter]
    public TimeSpan? SelectedTime { get; set; }
    
    [Parameter]
    public EventCallback<TimeSpan?> SelectedTimeChanged { get; set; }
    
    [Parameter]
    public string Placeholder { get; set; } = ""Pick a time"";
    
    [Parameter]
    public int Step { get; set; } = 15;
    
    [Parameter]
    public bool Disabled { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private bool _isOpen;
    private string selectedHour = ""0"";
    private string selectedMinute = ""0"";
    
    protected override void OnParametersSet()
    {
        if (SelectedTime.HasValue)
        {
            selectedHour = SelectedTime.Value.Hours.ToString();
            selectedMinute = SelectedTime.Value.Minutes.ToString();
        }
    }
    
    private void TogglePicker() => _isOpen = !_isOpen;
    
    private async Task ApplyTime()
    {
        var hour = int.Parse(selectedHour);
        var minute = int.Parse(selectedMinute);
        SelectedTime = new TimeSpan(hour, minute, 0);
        _isOpen = false;
        await SelectedTimeChanged.InvokeAsync(SelectedTime);
    }
}
";
}


