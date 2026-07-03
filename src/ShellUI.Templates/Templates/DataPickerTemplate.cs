using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DataPickerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "data-picker",
        DisplayName = "DataPicker",
        Description = "Generic typed picker with search, keyboard nav, and templated rendering",
        Category = ComponentCategory.Form,
        FilePath = "DataPicker.razor"
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@typeparam TItem
@typeparam TKey where TKey : notnull

<div class=""@Shell.Cn(""relative"", Class)"" @attributes=""AdditionalAttributes"">
    <button type=""button""
            @onclick=""Toggle""
            disabled=""@Disabled""
            class=""@Shell.Cn(""flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"", Disabled ? ""cursor-not-allowed opacity-50"" : """")"">
        <span class=""@Shell.Cn(""truncate text-left"", Value is null ? ""text-muted-foreground"" : """")"">
            @if (Value is not null && SelectedTemplate is not null)
            {
                @SelectedTemplate(Value)
            }
            else if (Value is not null)
            {
                @DisplayFor(Value)
            }
            else
            {
                @Placeholder
            }
        </span>
        <svg class=""@Shell.Cn(""h-4 w-4 opacity-50 transition-transform shrink-0"", IsOpen ? ""rotate-180"" : """")"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
        </svg>
    </button>

    @if (IsOpen)
    {
        <div class=""absolute z-50 mt-1 w-full rounded-md border border-border bg-popover shadow-md animate-in fade-in-0 zoom-in-95"">
            <div class=""flex items-center border-b border-border px-3"">
                <svg class=""mr-2 h-4 w-4 shrink-0 text-muted-foreground"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                    <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"" />
                </svg>
                <input @ref=""_searchInput""
                       type=""text""
                       @bind=""_searchQuery""
                       @bind:event=""oninput""
                       @onkeydown=""OnKeyDown""
                       placeholder=""@SearchPlaceholder""
                       class=""flex h-10 w-full bg-transparent py-2 text-sm outline-none placeholder:text-muted-foreground"" />
            </div>
            <div class=""max-h-60 overflow-auto p-1"">
                @{
                    var filtered = Filter().ToList();
                }
                @if (filtered.Count == 0)
                {
                    <div class=""py-6 text-center text-sm text-muted-foreground"">@EmptyText</div>
                }
                else
                {
                    for (var i = 0; i < filtered.Count; i++)
                    {
                        var item = filtered[i];
                        var isSelected = Value is not null && KeyFor(item).Equals(KeyFor(Value));
                        var isHighlighted = i == _highlightedIndex;
                        var localIdx = i;
                        <button type=""button""
                                @onclick=""() => SelectItem(item)""
                                @onmouseenter=""() => _highlightedIndex = localIdx""
                                class=""@Shell.Cn(""relative flex w-full cursor-pointer select-none items-center rounded-sm py-1.5 pl-2 pr-8 text-sm outline-none"", isHighlighted ? ""bg-accent text-accent-foreground"" : """", isSelected && !isHighlighted ? ""bg-accent/50"" : """")"">
                            @if (OptionTemplate is not null)
                            {
                                @OptionTemplate(item)
                            }
                            else
                            {
                                @DisplayFor(item)
                            }
                            @if (isSelected)
                            {
                                <span class=""absolute right-2 flex h-3.5 w-3.5 items-center justify-center"">
                                    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M5 13l4 4L19 7"" />
                                    </svg>
                                </span>
                            }
                        </button>
                    }
                }
            </div>
        </div>
    }
</div>

@if (IsOpen)
{
    <div class=""fixed inset-0 z-40"" @onclick=""Close""></div>
}

@code {
    [Parameter, EditorRequired] public IEnumerable<TItem> Items { get; set; } = Array.Empty<TItem>();
    [Parameter] public TItem? Value { get; set; }
    [Parameter] public EventCallback<TItem?> ValueChanged { get; set; }
    [Parameter, EditorRequired] public Func<TItem, TKey> KeySelector { get; set; } = default!;
    [Parameter] public Func<TItem, string>? DisplaySelector { get; set; }
    [Parameter] public Func<TItem, string, bool>? SearchPredicate { get; set; }
    [Parameter] public RenderFragment<TItem>? SelectedTemplate { get; set; }
    [Parameter] public RenderFragment<TItem>? OptionTemplate { get; set; }
    [Parameter] public string Placeholder { get; set; } = ""Select..."";
    [Parameter] public string SearchPlaceholder { get; set; } = ""Search..."";
    [Parameter] public string EmptyText { get; set; } = ""No results found."";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsOpen { get; set; }
    private string _searchQuery = """";
    private int _highlightedIndex;
    private ElementReference _searchInput;
    private bool _shouldFocus;

    private string DisplayFor(TItem item) => DisplaySelector?.Invoke(item) ?? item?.ToString() ?? """";
    private TKey KeyFor(TItem item) => KeySelector(item);

    private IEnumerable<TItem> Filter()
    {
        if (string.IsNullOrWhiteSpace(_searchQuery)) return Items;
        var q = _searchQuery;
        if (SearchPredicate is not null) return Items.Where(i => SearchPredicate(i, q));
        return Items.Where(i => DisplayFor(i).Contains(q, StringComparison.OrdinalIgnoreCase));
    }

    private void Toggle()
    {
        if (Disabled) return;
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            _searchQuery = """";
            _highlightedIndex = 0;
            _shouldFocus = true;
        }
    }

    private void Close() => IsOpen = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_shouldFocus)
        {
            _shouldFocus = false;
            try { await _searchInput.FocusAsync(); } catch { }
        }
    }

    private async Task SelectItem(TItem item)
    {
        Value = item;
        IsOpen = false;
        await ValueChanged.InvokeAsync(item);
    }

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        var filtered = Filter().ToList();
        if (e.Key == ""Escape"") { Close(); return; }
        if (e.Key == ""ArrowDown"") { _highlightedIndex = Math.Min(_highlightedIndex + 1, filtered.Count - 1); return; }
        if (e.Key == ""ArrowUp"") { _highlightedIndex = Math.Max(_highlightedIndex - 1, 0); return; }
        if (e.Key == ""Enter"" && filtered.Count > 0)
        {
            var idx = Math.Clamp(_highlightedIndex, 0, filtered.Count - 1);
            await SelectItem(filtered[idx]);
        }
    }
}
";
}
