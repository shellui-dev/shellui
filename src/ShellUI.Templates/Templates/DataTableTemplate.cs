namespace ShellUI.Templates.Templates;

public static class DataTableTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "data-table",
        Description = "Advanced data table with sorting, filtering, pagination, and row selection",
        Category = ComponentCategory.DataDisplay,
        Dependencies = new[] { "table", "input", "checkbox", "select", "button", "dropdown" }
    };

    public static string Content => @"
@typeparam TItem
@using System.Linq.Dynamic.Core

<div class=""w-full"">
    <!-- Filters -->
    @if (ShowFilters)
    {
        <div class=""flex items-center py-4"">
            <Input
                Placeholder=""Filter...""
                Value=""@_filterText""
                ValueChanged=""OnFilterChanged""
                Class=""max-w-sm"" />
        </div>
    }

    <!-- Table -->
    <div class=""rounded-md border overflow-x-auto"">
        <Table Class=""min-w-[640px]"">
            <TableHeader>
                <TableRow>
                    @if (ShowSelection)
                    {
                        <TableHead Class=""w-12"">
                            <Checkbox
                                Checked=""@(_selectedItems.Count == _filteredItems.Count && _filteredItems.Any())""
                                CheckedChanged=""OnSelectAllChanged"" />
                        </TableHead>
                    }
                    @foreach (var column in Columns)
                    {
                        <TableHead
                            Class=""@($""cursor-pointer select-none {(column.Sortable ? ""hover:bg-muted/50"" : """")}"")""
                            @onclick=""() => column.Sortable ? OnSortClicked(column) : Task.CompletedTask"">
                            <div class=""flex items-center gap-2"">
                                @column.Header
                                @if (column.Sortable)
                                {
                                    <svg class=""h-4 w-4 @(column.SortDirection == SortDirection.Ascending ? ""text-foreground"" : column.SortDirection == SortDirection.Descending ? ""text-foreground rotate-180"" : ""text-muted-foreground"")""
                                         xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                                        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M7 16V4m0 0L3 8m4-4l4 4m6 0v12m0 0l4-4m-4 4l-4-4"" />
                                    </svg>
                                }
                            </div>
                        </TableHead>
                    }
                    @if (RowActions?.Any() == true)
                    {
                        <TableHead>Actions</TableHead>
                    }
                </TableRow>
            </TableHeader>
            <TableBody>
                @if (_filteredItems.Any())
                {
                    @foreach (var item in _paginatedItems)
                    {
                        <TableRow Class=""@(IsSelected(item) ? ""bg-muted/50"" : """")"">
                            @if (ShowSelection)
                            {
                                <TableCell>
                                    <Checkbox
                                        Checked=""@IsSelected(item)""
                                        CheckedChanged=""(checked) => OnItemSelectionChanged(item, checked)"" />
                                </TableCell>
                            }
                            @foreach (var column in Columns)
                            {
                                <TableCell>@column.CellTemplate(item)</TableCell>
                            }
                            @if (RowActions?.Any() == true)
                            {
                                <TableCell>
                                    <Dropdown>
                                        <DropdownTrigger>
                                            <Button Variant=""ghost"" Size=""sm"">
                                                <svg class=""h-4 w-4"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                                                    <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 5v.01M12 12v.01M12 19v.01M12 6a1 1 0 110-2 1 1 0 010 2zm0 7a1 1 0 110-2 1 1 0 010 2zm0 7a1 1 0 110-2 1 1 0 010 2z"" />
                                                </svg>
                                            </Button>
                                        </DropdownTrigger>
                                        <DropdownContent>
                                            @foreach (var action in RowActions)
                                            {
                                                <button class=""w-full text-left px-2 py-1.5 text-sm hover:bg-accent hover:text-accent-foreground rounded-sm""
                                                        @onclick=""() => action.Action(item)"">
                                                    @action.Label
                                                </button>
                                            }
                                        </DropdownContent>
                                    </Dropdown>
                                </TableCell>
                            }
                        </TableRow>
                    }
                }
                else
                {
                    <TableRow>
                        <TableCell Colspan=""@(Columns.Count + (ShowSelection ? 1 : 0) + (RowActions?.Any() == true ? 1 : 0))""
                                   Class=""h-24 text-center"">
                            No results found.
                        </TableCell>
                    </TableRow>
                }
            </TableBody>
        </Table>
    </div>

    <!-- Pagination -->
    @if (ShowPagination && _filteredItems.Any())
    {
        <div class=""flex flex-col gap-4 px-2 py-4 sm:flex-row sm:items-center sm:justify-between"">
            <div class=""flex-1 text-sm text-muted-foreground"">
                @if (_selectedItems.Any())
                {
                    @_selectedItems.Count item(s) selected
                }
                else
                {
                    Showing @_startIndex - @_endIndex of @_filteredItems.Count results
                }
            </div>
            <div class=""flex flex-col gap-4 sm:flex-row sm:items-center sm:space-x-6 lg:space-x-8"">
                <div class=""flex items-center space-x-2"">
                    <p class=""text-sm font-medium whitespace-nowrap"">Rows per page</p>
                    <Select Value=""@PageSize.ToString()"" ValueChanged=""OnPageSizeChanged"">
                        <SelectTrigger Class=""h-8 w-[70px]"">
                            <SelectValue />
                        </SelectTrigger>
                        <SelectContent>
                            @foreach (var size in new[] { 5, 10, 20, 50 })
                            {
                                <SelectItem Value=""@size.ToString()"">@size</SelectItem>
                            }
                        </SelectContent>
                    </Select>
                </div>
                <div class=""flex w-[100px] items-center justify-center text-sm font-medium whitespace-nowrap"">
                    Page @_currentPage of @_totalPages
                </div>
                <div class=""flex items-center space-x-2"">
                    <Button
                        Variant=""outline""
                        Size=""sm""
                        Disabled=""@(_currentPage <= 1)""
                        Class=""whitespace-nowrap""
                        @onclick=""OnPreviousPage"">
                        Previous
                    </Button>
                    <Button
                        Variant=""outline""
                        Size=""sm""
                        Disabled=""@(_currentPage >= _totalPages)""
                        Class=""whitespace-nowrap""
                        @onclick=""OnNextPage"">
                        Next
                    </Button>
                </div>
            </div>
        </div>
    }
</div>

@code {
    [Parameter]
    public IEnumerable<TItem> Items { get; set; } = new List<TItem>();

    [Parameter]
    public List<DataTableColumn<TItem>> Columns { get; set; } = new();

    [Parameter]
    public List<DataTableAction<TItem>>? RowActions { get; set; }

    [Parameter]
    public bool ShowFilters { get; set; } = true;

    [Parameter]
    public bool ShowPagination { get; set; } = true;

    [Parameter]
    public bool ShowSelection { get; set; } = false;

    [Parameter]
    public int PageSize { get; set; } = 10;

    [Parameter]
    public EventCallback<HashSet<TItem>> SelectedItemsChanged { get; set; }

    private string _filterText = """";
    private IEnumerable<TItem> _filteredItems = new List<TItem>();
    private IEnumerable<TItem> _paginatedItems = new List<TItem>();
    private HashSet<TItem> _selectedItems = new();
    private int _currentPage = 1;
    private int _totalPages = 1;
    private int _startIndex = 1;
    private int _endIndex = 1;

    protected override void OnParametersSet()
    {
        UpdateFilteredItems();
    }

    private void UpdateFilteredItems()
    {
        // Apply filtering
        _filteredItems = string.IsNullOrWhiteSpace(_filterText)
            ? Items
            : Items.Where(item =>
                Columns.Any(col => col.FilterPredicate?.Invoke(item, _filterText) == true));

        // Apply sorting
        var sortColumn = Columns.FirstOrDefault(c => c.SortDirection != SortDirection.None);
        if (sortColumn != null)
        {
            var sortDirection = sortColumn.SortDirection == SortDirection.Ascending ? """" : ""descending"";
            _filteredItems = _filteredItems.AsQueryable().OrderBy($""{sortColumn.PropertyName} {sortDirection}"");
        }

        // Update pagination
        _totalPages = (int)Math.Ceiling(_filteredItems.Count() / (double)PageSize);
        _currentPage = Math.Min(_currentPage, Math.Max(1, _totalPages));
        _startIndex = (_currentPage - 1) * PageSize + 1;
        _endIndex = Math.Min(_currentPage * PageSize, _filteredItems.Count());
        _paginatedItems = _filteredItems.Skip((_currentPage - 1) * PageSize).Take(PageSize);

        StateHasChanged();
    }

    private void OnFilterChanged(string value)
    {
        _filterText = value;
        _currentPage = 1; // Reset to first page
        UpdateFilteredItems();
    }

    private void OnSortClicked(DataTableColumn<TItem> column)
    {
        if (!column.Sortable) return;

        // Reset other columns
        foreach (var col in Columns.Where(c => c != column))
        {
            col.SortDirection = SortDirection.None;
        }

        // Cycle sort direction
        column.SortDirection = column.SortDirection switch
        {
            SortDirection.None => SortDirection.Ascending,
            SortDirection.Ascending => SortDirection.Descending,
            SortDirection.Descending => SortDirection.None,
            _ => SortDirection.Ascending
        };

        UpdateFilteredItems();
    }

    private void OnSelectAllChanged(bool selected)
    {
        if (selected)
        {
            _selectedItems = new HashSet<TItem>(_filteredItems);
        }
        else
        {
            _selectedItems.Clear();
        }

        SelectedItemsChanged.InvokeAsync(_selectedItems);
        StateHasChanged();
    }

    private void OnItemSelectionChanged(TItem item, bool selected)
    {
        if (selected)
        {
            _selectedItems.Add(item);
        }
        else
        {
            _selectedItems.Remove(item);
        }

        SelectedItemsChanged.InvokeAsync(_selectedItems);
        StateHasChanged();
    }

    private bool IsSelected(TItem item) => _selectedItems.Contains(item);

    private void OnPageSizeChanged(string value)
    {
        if (int.TryParse(value, out var size))
        {
            PageSize = size;
            _currentPage = 1;
            UpdateFilteredItems();
        }
    }

    private void OnPreviousPage()
    {
        if (_currentPage > 1)
        {
            _currentPage--;
            UpdateFilteredItems();
        }
    }

    private void OnNextPage()
    {
        if (_currentPage < _totalPages)
        {
            _currentPage++;
            UpdateFilteredItems();
        }
    }
}

public class DataTableColumn<TItem>
{
    public string Header { get; set; } = """";
    public string PropertyName { get; set; } = """";
    public bool Sortable { get; set; } = true;
    public SortDirection SortDirection { get; set; } = SortDirection.None;
    public RenderFragment<TItem> CellTemplate { get; set; } = null!;
    public Func<TItem, string, bool>? FilterPredicate { get; set; }
}

public class DataTableAction<TItem>
{
    public string Label { get; set; } = """";
    public Action<TItem> Action { get; set; } = null!;
}

public enum SortDirection
{
    None,
    Ascending,
    Descending
}";
}
