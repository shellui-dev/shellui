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
        Dependencies = new List<string> { "stepper-list", "stepper-step", "stepper-content" }
    };

    public const string Content = """
@namespace YourProjectNamespace.Components.UI

<CascadingValue Value="this" IsFixed="true">
    <div class="@Shell.Cn("w-full", Class)">
        @ChildContent
        @if (ShowNavigation)
        {
            @if (NavigationContent != null)
            {
                <div class="mt-6">@NavigationContent</div>
            }
            else
            {
            <div class="mt-6 flex justify-between">
                <button type="button" class="@Shell.Cn("inline-flex items-center justify-center rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 h-10 px-4 py-2", CurrentStep > 0 ? "bg-primary text-primary-foreground hover:bg-primary/90" : "bg-muted text-muted-foreground")"
                        disabled="@(CurrentStep == 0)"
                        @onclick="PreviousStep">
                    @PreviousText
                </button>
                @if (IsLastStep && ShowConfirmOnLast)
                {
                    <button type="button" class="inline-flex items-center justify-center rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 bg-primary text-primary-foreground hover:bg-primary/90 h-10 px-4 py-2"
                            @onclick="HandleConfirm">
                        @ConfirmText
                    </button>
                }
                else
                {
                    <button type="button" class="@Shell.Cn("inline-flex items-center justify-center rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 h-10 px-4 py-2", !IsLastStep ? "bg-primary text-primary-foreground hover:bg-primary/90" : "bg-muted text-muted-foreground")"
                            disabled="@IsLastStep"
                            @onclick="NextStep">
                        @NextText
                    </button>
                }
            </div>
            }
        }
    </div>
</CascadingValue>

@code {
    [Parameter] public int TotalSteps { get; set; } = 3;
    [Parameter] public int CurrentStep { get; set; } = 0;
    [Parameter] public EventCallback<int> CurrentStepChanged { get; set; }
    [Parameter] public EventCallback OnConfirm { get; set; }
    [Parameter] public bool ShowNavigation { get; set; } = true;
    [Parameter] public bool ShowConfirmOnLast { get; set; } = true;
    [Parameter] public string ConfirmText { get; set; } = "Confirm";
    [Parameter] public string NextText { get; set; } = "Next";
    [Parameter] public string PreviousText { get; set; } = "Previous";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? NavigationContent { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;

    private int _highestStepReached = 0;
    private bool IsLastStep => CurrentStep >= TotalSteps - 1;

    public int HighestStepReached => _highestStepReached;
    public int StepCount => TotalSteps;

    protected override void OnParametersSet()
    {
        if (CurrentStep > _highestStepReached)
            _highestStepReached = CurrentStep;
    }

    public async Task GoToStepAsync(int step)
    {
        if (step > _highestStepReached || step < 0) return;
        CurrentStep = step;
        await CurrentStepChanged.InvokeAsync(step);
    }

    private async Task PreviousStep()
    {
        if (CurrentStep > 0)
            await GoToStepAsync(CurrentStep - 1);
    }

    private async Task NextStep()
    {
        if (CurrentStep < TotalSteps - 1)
        {
            var nextStep = CurrentStep + 1;
            if (nextStep > _highestStepReached)
                _highestStepReached = nextStep;
            CurrentStep = nextStep;
            await CurrentStepChanged.InvokeAsync(nextStep);
        }
    }

    private async Task HandleConfirm()
    {
        if (TotalSteps > 0 && CurrentStep >= TotalSteps - 1)
        {
            if (_highestStepReached < TotalSteps - 1)
                _highestStepReached = TotalSteps - 1;
            await OnConfirm.InvokeAsync();
        }
    }
}
""";
}
