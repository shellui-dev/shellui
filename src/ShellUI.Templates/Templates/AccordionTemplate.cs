using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AccordionTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion",
        DisplayName = "Accordion",
        Description = "Collapsible content sections",
        Category = ComponentCategory.Layout,
        FilePath = "Accordion.razor",

        Tags = new List<string> { "layout", "collapsible", "accordion" },
        Dependencies = new List<string> { "accordion-type", "accordion-item" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@Shell.Cn(Class)"" @attributes=""AdditionalAttributes"">
    <CascadingValue Value=""this"">
        @ChildContent
    </CascadingValue>
</div>

@code {
    [Parameter] public AccordionType Type { get; set; } = AccordionType.Single;
    [Parameter] public bool Collapsible { get; set; } = true;
    [Parameter] public string? DefaultValue { get; set; }
    [Parameter] public string? Value { get; set; }
    [Parameter] public EventCallback<string?> ValueChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string? _openValue;
    private HashSet<string> _openValues = new();
    private List<AccordionItem> _items = new();
    private bool _initialized;

    internal string? OpenValue => Value ?? _openValue;

    public void RegisterItem(AccordionItem item)
    {
        var idx = _items.Count;
        if (string.IsNullOrEmpty(item.Value))
            item.SetAssignedValue($""item-{idx}"");
        _items.Add(item);
        if (!_initialized)
        {
            _initialized = true;
            var dv = Value ?? DefaultValue;
            if (Type == AccordionType.Single)
            {
                if (!string.IsNullOrEmpty(dv))
                    _openValue = dv;
                else if (item.IsOpen)
                    _openValue = item.EffectiveValue;
            }
            else if (Type == AccordionType.Multiple && !string.IsNullOrEmpty(DefaultValue))
                _openValues = new HashSet<string>(DefaultValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }
    }

    public void UnregisterItem(AccordionItem item)
    {
        _items.Remove(item);
    }

    internal bool IsItemOpen(string? value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        return Type == AccordionType.Single
            ? OpenValue == value
            : _openValues.Contains(value);
    }

    internal async Task ToggleItemAsync(AccordionItem item)
    {
        var v = item.EffectiveValue;
        if (string.IsNullOrEmpty(v)) return;

        if (Type == AccordionType.Single)
        {
            var isCurrentlyOpen = OpenValue == v;
            if (isCurrentlyOpen && !Collapsible) return;
            var newValue = isCurrentlyOpen ? null : v;
            _openValue = newValue;
            if (ValueChanged.HasDelegate)
                await ValueChanged.InvokeAsync(newValue);
        }
        else
        {
            if (_openValues.Contains(v))
                _openValues.Remove(v);
            else
                _openValues.Add(v);
        }
        StateHasChanged();
    }
}
";
}


