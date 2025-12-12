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

