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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""flex items-start justify-between w-full"">
    @ChildContent
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
";
}
