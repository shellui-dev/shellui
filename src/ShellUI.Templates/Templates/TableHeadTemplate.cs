using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class TableHeadTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "table-head",
        DisplayName = "Table Head",
        Description = "Table header cell component",
        Category = ComponentCategory.DataDisplay,
        FilePath = "TableHead.razor",
        IsAvailable = false,
        Tags = new List<string> { "table", "header", "th" }
    };

    public static string Content => ComponentSource.Read("Components/TableHead.razor");
}


