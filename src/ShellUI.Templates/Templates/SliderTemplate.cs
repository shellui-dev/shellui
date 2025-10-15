using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SliderTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "slider",
        DisplayName = "Slider",
        Description = "Range slider input component",
        Category = ComponentCategory.Form,
        FilePath = "Slider.razor",
        Version = "0.1.0",
        Tags = new List<string> { "form", "input", "range", "slider" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""relative flex w-full touch-none select-none items-center @ClassName"" @attributes=""AdditionalAttributes"">
    <input type=""range""
           min=""@Min""
           max=""@Max""
           step=""@Step""
           value=""@Value""
           @oninput=""HandleInput""
           disabled=""@Disabled""
           class=""h-2 w-full cursor-pointer appearance-none rounded-full bg-secondary disabled:cursor-not-allowed disabled:opacity-50 [&::-webkit-slider-thumb]:appearance-none [&::-webkit-slider-thumb]:h-5 [&::-webkit-slider-thumb]:w-5 [&::-webkit-slider-thumb]:rounded-full [&::-webkit-slider-thumb]:bg-primary [&::-webkit-slider-thumb]:ring-offset-background [&::-webkit-slider-thumb]:transition-colors focus-visible:[&::-webkit-slider-thumb]:ring-2 focus-visible:[&::-webkit-slider-thumb]:ring-ring focus-visible:[&::-webkit-slider-thumb]:ring-offset-2 [&::-moz-range-thumb]:h-5 [&::-moz-range-thumb]:w-5 [&::-moz-range-thumb]:rounded-full [&::-moz-range-thumb]:bg-primary [&::-moz-range-thumb]:ring-offset-background [&::-moz-range-thumb]:transition-colors focus-visible:[&::-moz-range-thumb]:ring-2 focus-visible:[&::-moz-range-thumb]:ring-ring focus-visible:[&::-moz-range-thumb]:ring-offset-2"" />
</div>

@code {
    [Parameter]
    public double Value { get; set; } = 50;
    
    [Parameter]
    public EventCallback<double> ValueChanged { get; set; }
    
    [Parameter]
    public double Min { get; set; } = 0;
    
    [Parameter]
    public double Max { get; set; } = 100;
    
    [Parameter]
    public double Step { get; set; } = 1;
    
    [Parameter]
    public bool Disabled { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private async Task HandleInput(ChangeEventArgs args)
    {
        if (double.TryParse(args.Value?.ToString(), out var newValue))
        {
            Value = newValue;
            await ValueChanged.InvokeAsync(newValue);
        }
    }
}
";
}

