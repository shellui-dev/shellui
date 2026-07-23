using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TypedSelectTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "typed-select",
        DisplayName = "TypedSelect",
        Description = "Generic <select> that binds to any type — string, enum (nullable), Guid, bool, int, decimal — via InvariantCulture round-tripping. No external font dependency.",
        Category = ComponentCategory.Form,
        FilePath = "TypedSelect.razor"
    };

    public static string Content => """"
@namespace YourProjectNamespace.Components.UI
@typeparam TValue
@using System.Globalization

<div class="relative w-full">
    <select value="@FormatValue()"
            @onchange="OnChange"
            disabled="@Disabled"
            class="@ComputedClass"
            @attributes="AdditionalAttributes">
        @ChildContent
    </select>
    <div class="pointer-events-none absolute inset-y-0 right-0 flex items-center pr-3">
        <svg class="h-4 w-4 text-foreground/70" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 9l-7 7-7-7" />
        </svg>
    </div>
</div>

@code {
    [Parameter] public TValue? Value { get; set; }
    [Parameter] public EventCallback<TValue?> ValueChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string ComputedClass => Shell.Cn(
        "flex h-10 w-full appearance-none rounded-md border border-input bg-background pl-3 pr-9 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50",
        Class
    );

    private string? FormatValue() => Value switch
    {
        null => null,
        IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
        _ => Value.ToString()
    };

    private async Task OnChange(ChangeEventArgs e)
    {
        var raw = e.Value?.ToString();
        if (raw is null)
        {
            await ValueChanged.InvokeAsync(default);
            return;
        }

        var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);

        try
        {
            object? parsed = targetType switch
            {
                _ when targetType == typeof(string) => raw,
                _ when targetType.IsEnum => string.IsNullOrEmpty(raw) ? null : Enum.Parse(targetType, raw),
                _ when targetType == typeof(Guid) => string.IsNullOrEmpty(raw) ? Guid.Empty : Guid.Parse(raw),
                _ when targetType == typeof(bool) => bool.Parse(raw),
                _ when string.IsNullOrEmpty(raw) => null,
                _ => Convert.ChangeType(raw, targetType, CultureInfo.InvariantCulture)
            };
            await ValueChanged.InvokeAsync(parsed is null ? default : (TValue?)parsed);
        }
        catch
        {
            await ValueChanged.InvokeAsync(default);
        }
    }
}
"""";
}
