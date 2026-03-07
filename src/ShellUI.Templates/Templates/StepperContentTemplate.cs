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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (ParentStepper != null && ParentStepper.CurrentStep == Index)
{
    <div class=""mt-8"">
        @ChildContent
    </div>
}

@code {
    [CascadingParameter] private Stepper? ParentStepper { get; set; }
    [Parameter] public string Value { get; set; } = ""0"";
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private int Index => int.TryParse(Value, out var v) ? v : 0;
}
";
}
