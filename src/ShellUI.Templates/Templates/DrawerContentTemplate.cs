using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class DrawerContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "drawer-content",
        DisplayName = "Drawer Content",
        Description = "Content subcomponent for the compositional Drawer pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "DrawerContent.razor",
        IsAvailable = false,
        Dependencies = new List<string>(),
        Tags = new List<string> { "overlay", "drawer", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using YourProjectNamespace.Components.UI.Variants

@if (Parent?.Open == true)
{
    <div class=""fixed inset-0 z-50 bg-black/80 animate-in fade-in-0"" @onclick=""Close""></div>
    <div class=""@DrawerVariants.Get(Parent.Side, Class)"" @onclick:stopPropagation=""true"" @attributes=""AdditionalAttributes"">
        <div class=""mx-auto mt-4 h-2 w-[100px] rounded-full bg-muted""></div>
        <div class=""flex flex-col gap-4 p-4"">
            @ChildContent
        </div>
    </div>
}

@code {
    [CascadingParameter] private Drawer? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Close()
    {
        if (Parent != null) await Parent.SetOpen(false);
    }
}
";
}
