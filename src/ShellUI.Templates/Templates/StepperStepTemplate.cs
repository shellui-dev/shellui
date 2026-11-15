using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class StepperStepTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "stepper-step",
        DisplayName = "Stepper Step",
        Description = "Step indicator (circle, title, description)",
        Category = ComponentCategory.Navigation,
        FilePath = "StepperStep.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/StepperStep.razor");
}
