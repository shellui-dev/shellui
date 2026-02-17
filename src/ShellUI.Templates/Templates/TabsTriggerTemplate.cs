using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs-trigger",
        DisplayName = "Tabs Trigger",
        Description = "Tab trigger button",
        Category = ComponentCategory.Navigation,

        FilePath = "TabsTrigger.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@{
    var isActive = ParentTabs?.CurrentValue == Value;
}
<button type=""button""
        @onclick=""OnClick""
        disabled=""@Disabled""
        class=""@Shell.Cn(""inline-flex items-center justify-center whitespace-nowrap rounded-md px-3 py-1.5 text-sm font-medium ring-offset-background transition-all focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"", isActive ? ""bg-background text-foreground shadow-sm"" : ""hover:text-foreground"")""
        @attributes=""AdditionalAttributes"">
    @ChildContent
</button>

@code {
    [CascadingParameter] private Tabs? ParentTabs { get; set; }
    [Parameter] public string Value { get; set; } = """";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task OnClick()
    {
        if (ParentTabs != null && !Disabled)
        {
            await ParentTabs.SetValueAsync(Value);
        }
    }
}
";
}
