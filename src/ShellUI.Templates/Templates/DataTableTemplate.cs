using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class DataTableTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "data-table",
        DisplayName = "Data Table",
        Description = "Advanced data table with sorting, filtering, and pagination",
        Category = ComponentCategory.DataDisplay,
        FilePath = "DataTable.razor",
        Dependencies = new List<string> { "data-table-models" },
        NuGetDependencies = new List<NuGetDependency>
        {
            new() { PackageId = "System.Linq.Dynamic.Core", Version = "1.7.1" }
        }
    };

    public static string Content => ComponentSource.Read("Components/DataTable.razor");
}

