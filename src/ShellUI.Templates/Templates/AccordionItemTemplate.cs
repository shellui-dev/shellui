using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class AccordionItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion-item",
        DisplayName = "Accordion Item",
        Description = "Individual collapsible section within an Accordion",
        Category = ComponentCategory.Layout,
        FilePath = "AccordionItem.razor",

        Tags = new List<string> { "layout", "collapsible", "item" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""border-b border-border @ClassName"" @attributes=""AdditionalAttributes"">
    <button type=""button""
            @onclick=""Toggle""
            class=""flex w-full flex-1 items-center justify-between py-4 font-medium transition-all hover:underline"">
        @Title
        <svg class=""@(""h-4 w-4 shrink-0 transition-transform duration-200 "" + (IsOpen ? ""rotate-180"" : """"))"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
        </svg>
    </button>
    @if (IsOpen)
    {
        <div class=""pb-4 pt-0"">
            @ChildContent
        </div>
    }
</div>

@code {
    [CascadingParameter]
    private Accordion? Accordion { get; set; }
    
    [Parameter]
    public string Title { get; set; } = """";
    
    [Parameter]
    public bool IsOpen { get; set; }
    
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    protected override void OnInitialized()
    {
        Accordion?.RegisterItem(this);
    }
    
    public void Dispose()
    {
        Accordion?.UnregisterItem(this);
    }
    
    private void Toggle()
    {
        IsOpen = !IsOpen;
    }
}
";
}


