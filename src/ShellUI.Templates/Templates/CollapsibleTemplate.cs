using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CollapsibleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "collapsible",
        DisplayName = "Collapsible",
        Description = "Collapsible content component",
        Category = ComponentCategory.Layout,
        FilePath = "Collapsible.razor",

        Tags = new List<string> { "layout", "collapsible", "toggle", "expand" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@(""space-y-2 "" + ClassName)"" @attributes=""AdditionalAttributes"">
    <button type=""button""
            @onclick=""Toggle""
            class=""flex items-center justify-between w-full p-3 rounded-md hover:bg-accent transition-colors"">
        <span class=""font-medium"">@Title</span>
        <svg class=""@(""h-4 w-4 transition-transform "" + (IsOpen ? ""rotate-180"" : """"))""
             fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
        </svg>
    </button>
    
    @if (IsOpen)
    {
        <div class=""p-3 rounded-md border bg-card"">
            @ChildContent
        </div>
    }
</div>

@code {
    [Parameter]
    public bool IsOpen { get; set; }
    
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }
    
    [Parameter]
    public string Title { get; set; } = ""Collapsible"";
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private async Task Toggle()
    {
        IsOpen = !IsOpen;
        await IsOpenChanged.InvokeAsync(IsOpen);
    }
}
";
}


