using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DataTableModelsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "data-table-models",
        DisplayName = "DataTable Models",
        Description = "Models for DataTable component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "Models/DataTableModels.cs",
        Version = "0.1.0",
        Dependencies = new List<string>()
    };

    public static string Content => @"using Microsoft.AspNetCore.Components;

namespace YourProjectNamespace.Components.Models;

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
}
";
}

