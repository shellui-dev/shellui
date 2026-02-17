using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class PopoverTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "popover-trigger",
        DisplayName = "Popover Trigger",
        Description = "Trigger for Popover (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "PopoverTrigger.razor",
        Dependencies = new List<string> { "popover" },
        IsAvailable = false,
        Tags = new List<string> { "overlay", "popover", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onclick=""Toggle"" @attributes=""AdditionalAttributes"" role=""button"" tabindex=""0"" @onkeydown=""HandleKeyDown"">
    @ChildContent
</div>

@code {
    [CascadingParameter] public Popover? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Toggle()
    {
        if (Parent != null)
            await Parent.ToggleAsync();
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key is ""Enter"" or "" "" && Parent != null)
            await Parent.ToggleAsync();
    }
}
";
}
