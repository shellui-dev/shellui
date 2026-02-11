using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CollapsibleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "collapsible",
        DisplayName = "Collapsible",
        Description = "Collapsible content component with Trigger and CollapsibleContent",
        Category = ComponentCategory.Layout,
        FilePath = "Collapsible.razor",
        Dependencies = new List<string> { "collapsible-trigger", "collapsible-content" },
        Tags = new List<string> { "layout", "collapsible", "toggle", "expand" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<CascadingValue Value=""this"" IsFixed=""true"">
    <div class=""@Shell.Cn(Class)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
</CascadingValue>

@code {
    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    public async Task ToggleAsync()
    {
        IsOpen = !IsOpen;
        await IsOpenChanged.InvokeAsync(IsOpen);
    }
}
";
}
