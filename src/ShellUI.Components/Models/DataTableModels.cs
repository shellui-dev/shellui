using Microsoft.AspNetCore.Components;

namespace ShellUI.Components.Models;

public class DataTableColumn<TItem>
{
    public string Header { get; set; } = "";
    public string PropertyName { get; set; } = "";
    public bool Sortable { get; set; } = true;
    public SortDirection SortDirection { get; set; } = SortDirection.None;
    public RenderFragment<TItem> CellTemplate { get; set; } = null!;
    public Func<TItem, string, bool>? FilterPredicate { get; set; }
}

public class DataTableAction<TItem>
{
    public string Label { get; set; } = "";
    public Action<TItem> Action { get; set; } = null!;
}

public enum SortDirection
{
    None,
    Ascending,
    Descending
}

// Passed to OnDataRequest when DataTable is in ServerSide mode. The server-side data
// loader receives filter / sort / page state and returns just the current page.
public class DataTableRequest
{
    public int Skip { get; set; }
    public int Take { get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.None;
    public string SearchQuery { get; set; } = "";
}

public class DataTableResponse<TItem>
{
    public IEnumerable<TItem> Items { get; set; } = Array.Empty<TItem>();
    public int TotalCount { get; set; }
}
