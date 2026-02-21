using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class SheetContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sheet-content",
        DisplayName = "Sheet Content",
        Description = "Content subcomponent for the compositional Sheet pattern",
        Category = ComponentCategory.Overlay,
        FilePath = "SheetContent.razor",
        Dependencies = new List<string> { "sheet" },
        Tags = new List<string> { "overlay", "sheet", "content" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (Parent?.Open == true)
{
    <div class=""fixed inset-0 z-50 bg-background/80 backdrop-blur-sm animate-in fade-in-0"" @onclick=""Close""></div>
    <div class=""@SheetVariants.Get(Parent.Side, Class)"" @attributes=""AdditionalAttributes"">
        <div class=""flex flex-col gap-4 p-6"">
            @ChildContent
        </div>
    </div>
}

@code {
    [CascadingParameter] private Sheet? Parent { get; set; }
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
