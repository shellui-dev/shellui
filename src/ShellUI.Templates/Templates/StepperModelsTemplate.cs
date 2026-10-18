using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class StepperModelsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "stepper-models",
        DisplayName = "Stepper Models",
        Description = "Models for Stepper",
        Category = ComponentCategory.Navigation,
        FilePath = "Models/StepperModels.cs",
        IsAvailable = false,
        Dependencies = new List<string>()
    };

    public static string Content => @"using Microsoft.AspNetCore.Components;

namespace YourProjectNamespace.Components.Models;

public class StepItem
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public RenderFragment? Content { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsOptional { get; set; }
}
";
}
