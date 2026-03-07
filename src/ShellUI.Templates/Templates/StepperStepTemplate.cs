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

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@{
    var index = int.TryParse(Value, out var v) ? v : 0;
    var isActive = ParentStepper?.CurrentStep == index;
    var isCompleted = ParentStepper != null && index < ParentStepper.HighestStepReached;
    var isClickable = ParentStepper != null && index <= ParentStepper.HighestStepReached;
}
<div class=""flex flex-col items-center relative"">
    <button type=""button""
            class=""@Shell.Cn(""flex h-10 w-10 items-center justify-center rounded-full border-2 text-sm font-medium transition-colors"", isCompleted ? ""border-primary bg-primary text-primary-foreground"" : isActive ? ""border-primary bg-background text-primary"" : ""border-muted bg-background text-muted-foreground"", isClickable ? ""cursor-pointer hover:border-primary/50"" : ""cursor-not-allowed"")""
            disabled=""@(!isClickable)""
            @onclick=""async () => { if (ParentStepper != null) await ParentStepper.GoToStepAsync(index); }"">
        @if (isCompleted)
        {
            <svg class=""h-4 w-4"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M5 13l4 4L19 7"" />
            </svg>
        }
        else
        {
            @(index + 1)
        }
    </button>
    <div class=""mt-4 text-center"">
        <p class=""@Shell.Cn(""text-sm font-medium mb-2"", isActive || isCompleted ? ""text-foreground"" : ""text-muted-foreground"")"">@Title</p>
        @if (!string.IsNullOrEmpty(Description))
        {
            <p class=""text-xs text-muted-foreground mb-1"">@Description</p>
        }
    </div>
    @if (ParentStepper != null && Index < ParentStepper.StepCount - 1)
    {
        <div class=""@Shell.Cn(""absolute top-5 left-1/2 h-px w-24 -translate-y-1/2"", isCompleted ? ""bg-primary"" : ""bg-muted"")"" style=""transform: translateX(50%);""></div>
    }
</div>

@code {
    [CascadingParameter] private Stepper? ParentStepper { get; set; }
    [Parameter] public string Value { get; set; } = ""0"";
    [Parameter] public string Title { get; set; } = """";
    [Parameter] public string? Description { get; set; }

    private int Index => int.TryParse(Value, out var v) ? v : 0;
}
";
}
