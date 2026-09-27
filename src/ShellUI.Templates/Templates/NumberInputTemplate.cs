using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class NumberInputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "number-input",
        DisplayName = "Number Input",
        Description = "Numeric input with increment and decrement buttons",
        Category = ComponentCategory.Form,
        FilePath = "NumberInput.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "input", "number", "stepper" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using System.Globalization

<div class=""@Shell.Cn(""flex h-10 w-full items-center overflow-hidden rounded-md border border-input bg-background text-sm ring-offset-background focus-within:ring-2 focus-within:ring-ring focus-within:ring-offset-2"", Disabled ? ""opacity-50"" : """", Class)"">
    <button type=""button"" aria-label=""Decrease"" disabled=""@(Disabled || AtMin)"" @onclick=""DecrementAsync""
            class=""flex h-full w-10 shrink-0 items-center justify-center border-r border-input text-muted-foreground transition-colors hover:bg-muted hover:text-foreground disabled:pointer-events-none disabled:opacity-50"">
        <svg class=""h-4 w-4"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M5 12h14"" />
        </svg>
    </button>
    <input type=""number""
           inputmode=""decimal""
           value=""@Format(Value)""
           min=""@Format(Min)""
           max=""@Format(Max)""
           step=""@Format(Step)""
           disabled=""@Disabled""
           @onchange=""HandleChangeAsync""
           class=""h-full min-w-0 flex-1 bg-transparent px-2 text-center tabular-nums focus:outline-none [appearance:textfield] [&::-webkit-inner-spin-button]:appearance-none [&::-webkit-outer-spin-button]:appearance-none""
           @attributes=""AdditionalAttributes"" />
    <button type=""button"" aria-label=""Increase"" disabled=""@(Disabled || AtMax)"" @onclick=""IncrementAsync""
            class=""flex h-full w-10 shrink-0 items-center justify-center border-l border-input text-muted-foreground transition-colors hover:bg-muted hover:text-foreground disabled:pointer-events-none disabled:opacity-50"">
        <svg class=""h-4 w-4"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 5v14M5 12h14"" />
        </svg>
    </button>
</div>

@code {
    [Parameter] public decimal? Value { get; set; }
    [Parameter] public EventCallback<decimal?> ValueChanged { get; set; }
    [Parameter] public decimal? Min { get; set; }
    [Parameter] public decimal? Max { get; set; }
    [Parameter] public decimal Step { get; set; } = 1;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool AtMin => Min.HasValue && Value.HasValue && Value <= Min;
    private bool AtMax => Max.HasValue && Value.HasValue && Value >= Max;

    private static string? Format(decimal? value) => value?.ToString(CultureInfo.InvariantCulture);

    private Task DecrementAsync() => SetAsync((Value ?? Min ?? 0) - Step);
    private Task IncrementAsync() => SetAsync((Value ?? Min ?? 0) + Step);

    private Task HandleChangeAsync(ChangeEventArgs e)
    {
        var text = e.Value?.ToString();
        if (string.IsNullOrWhiteSpace(text)) return SetAsync(null);
        return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed)
            ? SetAsync(parsed)
            : SetAsync(Value);
    }

    private async Task SetAsync(decimal? value)
    {
        if (value.HasValue)
        {
            if (Min.HasValue && value < Min) value = Min;
            if (Max.HasValue && value > Max) value = Max;
        }
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }
}
";
}
