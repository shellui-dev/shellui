using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class HoverCardTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "hover-card-trigger",
        DisplayName = "Hover Card Trigger",
        Description = "Hover trigger for HoverCard (shadcn-style)",
        Category = ComponentCategory.Overlay,
        FilePath = "HoverCardTrigger.razor",
        Dependencies = new List<string> { "hover-card" },
        IsAvailable = false,
        Tags = new List<string> { "overlay", "hover-card", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div @onmouseover=""OnMouseOver"" @onmouseout=""OnMouseOut"" @attributes=""AdditionalAttributes"">
    @ChildContent
</div>

@code {
    [CascadingParameter] public HoverCard? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private void OnMouseOver()
    {
        Parent?.Show();
    }

    private void OnMouseOut()
    {
        Parent?.Hide();
    }
}
";
}
