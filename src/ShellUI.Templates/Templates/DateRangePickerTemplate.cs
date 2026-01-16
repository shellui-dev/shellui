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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@(""relative "" + ClassName)"" @attributes=""AdditionalAttributes"">
    <button type=""button""
            @onclick=""ToggleCalendar""
            disabled=""@Disabled""
            class=""@(""flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 "" + (Disabled ? ""opacity-50 cursor-not-allowed"" : """"))"">
        <span class=""@(StartDate == null && EndDate == null ? ""text-muted-foreground"" : """")"">
            @GetDisplayText()
        </span>
        <div class=""flex items-center gap-1"">
            @if (AllowClear && (StartDate.HasValue || EndDate.HasValue))
            {
                <button type=""button"" 
                        @onclick=""ClearRange"" 
                        @onclick:stopPropagation=""true""
                        class=""p-0.5 hover:bg-accent rounded transition-colors"">
                    <svg class=""h-3.5 w-3.5 text-muted-foreground hover:text-foreground"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M6 18L18 6M6 6l12 12"" />
                    </svg>
                </button>
            }
            <svg class=""h-4 w-4 text-muted-foreground"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z"" />
            </svg>
        </div>
    </button>
    
    @if (_isOpen)
    {
        <div class=""absolute z-50 mt-1 rounded-md border bg-popover p-3 text-popover-foreground shadow-md"">
            <div class=""flex items-center justify-between mb-2"">
                <button type=""button"" @onclick=""PreviousMonth"" class=""p-1 hover:bg-accent rounded"">
                    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M15 19l-7-7 7-7"" />
                    </svg>
                </button>
                <div class=""text-sm font-semibold"">@_currentMonth.ToString(""MMMM yyyy"")</div>
                <button type=""button"" @onclick=""NextMonth"" class=""p-1 hover:bg-accent rounded"">
                    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M9 5l7 7-7 7"" />
                    </svg>
                </button>
            </div>
            <div class=""grid grid-cols-7 gap-1"">
                <div class=""text-xs text-center text-muted-foreground font-medium"">Su</div>
                <div class=""text-xs text-center text-muted-foreground font-medium"">Mo</div>
                <div class=""text-xs text-center text-muted-foreground font-medium"">Tu</div>
                <div class=""text-xs text-center text-muted-foreground font-medium"">We</div>
                <div class=""text-xs text-center text-muted-foreground font-medium"">Th</div>
                <div class=""text-xs text-center text-muted-foreground font-medium"">Fr</div>
                <div class=""text-xs text-center text-muted-foreground font-medium"">Sa</div>
                @foreach (var day in GetCalendarDays())
                {
                    @if (day.HasValue)
                    {
                        <button type=""button""
                                @onclick=""@(() => SelectDate(day.Value))""
                                class=""@GetDateButtonClass(day.Value)"">
                            @day.Value.Day
                        </button>
                    }
                    else
                    {
                        <div class=""h-8 w-8""></div>
                    }
                }
            </div>
        </div>
    }
</div>

@code {
    [Parameter]
    public DateTime? StartDate { get; set; }
    
    [Parameter]
    public EventCallback<DateTime?> StartDateChanged { get; set; }
    
    [Parameter]
    public DateTime? EndDate { get; set; }
    
    [Parameter]
    public EventCallback<DateTime?> EndDateChanged { get; set; }
    
    [Parameter]
    public string Placeholder { get; set; } = ""Pick a date range"";
    
    [Parameter]
    public bool AllowClear { get; set; } = true;
    
    [Parameter]
    public bool Disabled { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private bool _isOpen;
    private DateTime _currentMonth = DateTime.Now;
    private bool _selectingStart = true;
    
    private string GetDisplayText()
    {
        if (StartDate.HasValue && EndDate.HasValue)
        {
            return $""{StartDate.Value:MMM dd} - {EndDate.Value:MMM dd, yyyy}"";
        }
        else if (StartDate.HasValue)
        {
            return $""{StartDate.Value:MMM dd, yyyy} - ..."";
        }
        return Placeholder;
    }
    
    private string GetDateButtonClass(DateTime date)
    {
        var baseClass = ""h-8 w-8 text-sm rounded-md hover:bg-accent transition-colors"";
        
        if (StartDate.HasValue && EndDate.HasValue)
        {
            if (date.Date == StartDate.Value.Date || date.Date == EndDate.Value.Date)
            {
                return baseClass + "" bg-primary text-primary-foreground"";
            }
            if (date > StartDate.Value && date < EndDate.Value)
            {
                return baseClass + "" bg-primary/20 text-primary-foreground"";
            }
        }
        else if (StartDate.HasValue && date.Date == StartDate.Value.Date)
        {
            return baseClass + "" bg-primary text-primary-foreground"";
        }
        
        return baseClass;
    }
    
    private void ToggleCalendar() => _isOpen = !_isOpen;
    private void PreviousMonth() => _currentMonth = _currentMonth.AddMonths(-1);
    private void NextMonth() => _currentMonth = _currentMonth.AddMonths(1);
    
    private async Task SelectDate(DateTime date)
    {
        if (_selectingStart || (!StartDate.HasValue && !EndDate.HasValue))
        {
            StartDate = date;
            EndDate = null;
            _selectingStart = false;
            await StartDateChanged.InvokeAsync(date);
        }
        else
        {
            if (date < StartDate)
            {
                EndDate = StartDate;
                StartDate = date;
                await StartDateChanged.InvokeAsync(date);
                await EndDateChanged.InvokeAsync(EndDate);
            }
            else
            {
                EndDate = date;
                await EndDateChanged.InvokeAsync(date);
            }
            _isOpen = false;
            _selectingStart = true;
        }
    }
    
    private async Task ClearRange()
    {
        StartDate = null;
        EndDate = null;
        _selectingStart = true;
        _isOpen = false;
        await StartDateChanged.InvokeAsync(null);
        await EndDateChanged.InvokeAsync(null);
    }
    
    private List<DateTime?> GetCalendarDays()
    {
        var days = new List<DateTime?>();
        var firstDay = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);
        var startDayOfWeek = (int)firstDay.DayOfWeek;
        
        for (int i = 0; i < startDayOfWeek; i++) days.Add(null);
        for (int day = 1; day <= lastDay.Day; day++) days.Add(new DateTime(_currentMonth.Year, _currentMonth.Month, day));
        
        return days;
    }
}
";
}

