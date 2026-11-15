using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class StatCardTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "stat-card",
        DisplayName = "Stat Card",
        Description = "KPI tile with value, change indicator and description",
        Category = ComponentCategory.DataDisplay,
        FilePath = "StatCard.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "stat", "kpi", "metric", "dashboard" }
    };

    public static string Content => ComponentSource.Read("Components/StatCard.razor");
}
