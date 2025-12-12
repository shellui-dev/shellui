using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabModelsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tab-models",
        DisplayName = "Tab Models",
        Description = "Models for Tabs component",
        Category = ComponentCategory.Navigation,
        Version = "0.1.0",
        FilePath = "Components/Models/TabModels.cs",
        IsAvailable = false
    };

    public static string Content => @"using Microsoft.AspNetCore.Components;

namespace YourProjectNamespace.Components.Models;

public class TabItem
{
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public string? Badge { get; set; }
    public RenderFragment? Content { get; set; }
    public bool Disabled { get; set; }
}
";
}

