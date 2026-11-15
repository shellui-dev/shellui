using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class StepperContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "stepper-content",
        DisplayName = "Stepper Content",
        Description = "Content panel for a step",
        Category = ComponentCategory.Navigation,
        FilePath = "StepperContent.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/StepperContent.razor");
}
