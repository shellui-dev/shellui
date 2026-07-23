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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using Microsoft.JSInterop
@implements IAsyncDisposable
@inject IJSRuntime JS

<div class=""@(""relative "" + ClassName)"" @attributes=""AdditionalAttributes"">
    <button type=""button""
            @onclick=""ToggleCalendar""
            disabled=""@Disabled""
            class=""@(""flex h-10 w-full items-center gap-2 rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 "" + (Disabled ? ""opacity-50 cursor-not-allowed"" : """"))"">
        <span class=""@(""flex-1 text-left "" + (SelectedDate == null ? ""text-muted-foreground"" : """"))"">
            @(SelectedDate?.ToString(""MMM dd, yyyy"") ?? Placeholder)
        </span>
        <div class=""ml-auto flex items-center gap-1 shrink-0"">
            @if (AllowClear && SelectedDate.HasValue)
            {
                <button type=""button""
                        @onclick=""ClearDate""
                        @onclick:stopPropagation=""true""
                        class=""p-0.5 hover:bg-accent rounded transition-colors"">
                    <svg class=""h-3.5 w-3.5 text-muted-foreground hover:text-foreground"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M6 18L18 6M6 6l12 12"" />
                    </svg>
                </button>
            }
            <svg class=""h-4 w-4 text-foreground/70"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
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
                                class=""@(""h-8 w-8 text-sm rounded-md hover:bg-accent "" + (day.Value.Date == SelectedDate?.Date ? ""bg-primary text-primary-foreground"" : """"))"">
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
    public DateTime? SelectedDate { get; set; }
    
    [Parameter]
    public EventCallback<DateTime?> SelectedDateChanged { get; set; }
    
    [Parameter]
    public string Placeholder { get; set; } = ""Pick a date"";
    
    [Parameter]
    public bool AllowClear { get; set; } = true;
    
    [Parameter]
    public bool Disabled { get; set; }

    /// Dismiss the calendar when the page scrolls. Matches shadcn/Radix behavior.
    /// Set false to keep the calendar open across scroll.
    [Parameter]
    public bool CloseOnScroll { get; set; }

    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private bool _isOpen;
    private DateTime _currentMonth = DateTime.Now;
    private DotNetObjectReference<DatePicker>? _selfRef;
    private readonly string _dismissHandle = Guid.NewGuid().ToString(""N"");
    private bool _dismissRegistered;

    private async Task ToggleCalendar()
    {
        _isOpen = !_isOpen;
        if (_isOpen) await RegisterDismissAsync();
        else await UnregisterDismissAsync();
    }

    [JSInvokable]
    public async Task OnDismissEvent()
    {
        if (_isOpen)
        {
            _isOpen = false;
            await UnregisterDismissAsync();
            StateHasChanged();
        }
    }

    private async Task RegisterDismissAsync()
    {
        if (!CloseOnScroll || _dismissRegistered) return;
        _selfRef ??= DotNetObjectReference.Create(this);
        try
        {
            await JS.InvokeVoidAsync(""ShellUI.onDismissEvents"", _dismissHandle, _selfRef);
            _dismissRegistered = true;
        }
        catch { }
    }

    private async Task UnregisterDismissAsync()
    {
        if (!_dismissRegistered) return;
        try { await JS.InvokeVoidAsync(""ShellUI.offDismissEvents"", _dismissHandle); } catch { }
        _dismissRegistered = false;
    }

    private void PreviousMonth() => _currentMonth = _currentMonth.AddMonths(-1);
    private void NextMonth() => _currentMonth = _currentMonth.AddMonths(1);

    private async Task SelectDate(DateTime date)
    {
        SelectedDate = date;
        _isOpen = false;
        await UnregisterDismissAsync();
        await SelectedDateChanged.InvokeAsync(date);
    }

    private async Task ClearDate()
    {
        SelectedDate = null;
        _isOpen = false;
        await UnregisterDismissAsync();
        await SelectedDateChanged.InvokeAsync(null);
    }

    public async ValueTask DisposeAsync()
    {
        await UnregisterDismissAsync();
        _selfRef?.Dispose();
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


