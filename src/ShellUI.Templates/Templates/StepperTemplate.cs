using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class StepperTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "stepper",
        DisplayName = "Stepper",
        Description = "Multi-step wizard with StepperList, StepperStep, StepperContent. Confirm button on last step.",
        Category = ComponentCategory.Navigation,
        FilePath = "Stepper.razor",
        Tags = new List<string> { "stepper", "steps", "progress", "wizard", "multi-step" },
        Dependencies = new List<string> { "stepper-models", "stepper-list", "stepper-step", "stepper-content" }
    };

    public static string Content => ComponentSource.Read("Components/Stepper.razor");
}
