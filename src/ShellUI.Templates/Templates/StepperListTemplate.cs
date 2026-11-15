using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class StepperListTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "stepper-list",
        DisplayName = "Stepper List",
        Description = "Container for step indicators",
        Category = ComponentCategory.Navigation,
        FilePath = "StepperList.razor",
        IsAvailable = false
    };

    public static string Content => ComponentSource.Read("Components/StepperList.razor");
}
