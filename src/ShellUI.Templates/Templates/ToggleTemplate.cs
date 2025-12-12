using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ToggleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "toggle",
        DisplayName = "Toggle",
        Description = "Toggle button with pressed state",
        Category = ComponentCategory.Form,
        FilePath = "Toggle.razor",
        Version = "0.1.0",
        Dependencies = new List<string> { "toggle-variants" },
        Tags = new List<string> { "form", "toggle", "button" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<button type=""button""
        @onclick=""HandleClick""
        disabled=""@Disabled""
        class=""@ComputedClass""
        @attributes=""AdditionalAttributes"">
    @ChildContent
</button>

@code {
    [Parameter] public bool Pressed { get; set; }
    [Parameter] public EventCallback<bool> PressedChanged { get; set; }
    [Parameter] public ToggleVariant Variant { get; set; } = ToggleVariant.Default;
    [Parameter] public ToggleSize Size { get; set; } = ToggleSize.Default;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private string ComputedClass => ToggleVariants.Get(Variant, Size, Pressed, Class);

    private async Task HandleClick()
    {
        Pressed = !Pressed;
        await PressedChanged.InvokeAsync(Pressed);
    }
}
";
}
