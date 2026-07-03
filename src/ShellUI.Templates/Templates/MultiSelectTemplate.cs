using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class MultiSelectTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "multi-select",
        DisplayName = "MultiSelect",
        Description = "Generic multi-value picker with chip display and search",
        Category = ComponentCategory.Form,
        FilePath = "MultiSelect.razor"
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@typeparam TItem
@typeparam TKey where TKey : notnull

<div class=""@Shell.Cn(""relative"", Class)"" @attributes=""AdditionalAttributes"">
    <button type=""button""
            @onclick=""Toggle""
            disabled=""@Disabled""
            class=""@Shell.Cn(""flex min-h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"", Disabled ? ""cursor-not-allowed opacity-50"" : """")"">
        <div class=""flex flex-1 flex-wrap gap-1"">
            @if (Values.Count == 0)
            {
                <span class=""text-muted-foreground"">@Placeholder</span>
            }
            else
            {
                foreach (var item in Values)
                {
                    var chip = item;
                    <span class=""inline-flex items-center gap-1 rounded-md bg-secondary px-2 py-0.5 text-xs font-medium text-secondary-foreground"">
                        @if (ChipTemplate is not null)
                        {
                            @ChipTemplate(chip)
                        }
                        else
                        {
                            @DisplayFor(chip)
                        }
                        <span role=""button""
                              tabindex=""0""
                              class=""cursor-pointer rounded-sm outline-none hover:text-destructive focus:ring-1 focus:ring-ring""
                              @onclick:stopPropagation=""true""
                              @onclick=""() => RemoveItem(chip)"">
                            <svg class=""h-3 w-3"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                                <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M6 18L18 6M6 6l12 12"" />
                            </svg>
                        </span>
                    </span>
                }
            }
        </div>
        <svg class=""@Shell.Cn(""ml-2 h-4 w-4 opacity-50 transition-transform shrink-0"", IsOpen ? ""rotate-180"" : """")"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
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
                        var isSelected = Values.Any(v => KeyFor(v).Equals(KeyFor(item)));
                        var isHighlighted = i == _highlightedIndex;
                        var localIdx = i;
                        <button type=""button""
                                @onclick=""() => ToggleItem(item)""
                                @onmouseenter=""() => _highlightedIndex = localIdx""
                                class=""@Shell.Cn(""relative flex w-full cursor-pointer select-none items-center rounded-sm py-1.5 pl-8 pr-2 text-sm outline-none"", isHighlighted ? ""bg-accent text-accent-foreground"" : """")"">
                            <span class=""absolute left-2 flex h-3.5 w-3.5 items-center justify-center"">
                                @if (isSelected)
                                {
                                    <svg class=""h-4 w-4"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M5 13l4 4L19 7"" />
                                    </svg>
                                }
                            </span>
                            @if (OptionTemplate is not null)
                            {
                                @OptionTemplate(item)
                            }
                            else
                            {
                                @DisplayFor(item)
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
    [Parameter] public List<TItem> Values { get; set; } = new();
    [Parameter] public EventCallback<List<TItem>> ValuesChanged { get; set; }
    [Parameter, EditorRequired] public Func<TItem, TKey> KeySelector { get; set; } = default!;
    [Parameter] public Func<TItem, string>? DisplaySelector { get; set; }
    [Parameter] public Func<TItem, string, bool>? SearchPredicate { get; set; }
    [Parameter] public RenderFragment<TItem>? ChipTemplate { get; set; }
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

    private async Task ToggleItem(TItem item)
    {
        var key = KeyFor(item);
        var existing = Values.FirstOrDefault(v => KeyFor(v).Equals(key));
        var next = new List<TItem>(Values);
        if (existing is not null && !existing.Equals(default(TItem))) next.Remove(existing);
        else next.Add(item);
        Values = next;
        await ValuesChanged.InvokeAsync(next);
    }

    private async Task RemoveItem(TItem item)
    {
        var next = Values.Where(v => !KeyFor(v).Equals(KeyFor(item))).ToList();
        Values = next;
        await ValuesChanged.InvokeAsync(next);
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
            await ToggleItem(filtered[idx]);
        }
    }
}
";
}
