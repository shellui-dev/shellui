using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs",
        DisplayName = "Tabs",
        Description = "Tabbed navigation with TabsList, TabsTrigger, TabsContent (shadcn-style)",
        Category = ComponentCategory.Navigation,

        FilePath = "Tabs.razor",
        Dependencies = new List<string> { "tabs-list", "tabs-trigger", "tabs-content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<CascadingValue Value=""this"" IsFixed=""true"">
    <div class=""@Shell.Cn(""w-full"", Class)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
</CascadingValue>

@code {
    [Parameter] public string Value { get; set; } = """";
    [Parameter] public string DefaultValue { get; set; } = """";
    [Parameter] public EventCallback<string> ValueChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string _effectiveValue = "";

    protected override void OnInitialized()
    {
        _effectiveValue = !string.IsNullOrEmpty(Value) ? Value : DefaultValue;
    }

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(Value))
            _effectiveValue = Value;
        else if (string.IsNullOrEmpty(_effectiveValue) && !string.IsNullOrEmpty(DefaultValue))
            _effectiveValue = DefaultValue;
    }

    public string CurrentValue => !string.IsNullOrEmpty(Value) ? Value : _effectiveValue;

    public async Task SetValueAsync(string newValue)
    {
        if (_effectiveValue == newValue) return;
        _effectiveValue = newValue;
        await ValueChanged.InvokeAsync(newValue);
        if (string.IsNullOrEmpty(Value))
            StateHasChanged();
    }
}
";
}
