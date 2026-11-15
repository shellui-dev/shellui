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

    public static string Content => ComponentSource.Read("Models/StepperModels.cs");
}
