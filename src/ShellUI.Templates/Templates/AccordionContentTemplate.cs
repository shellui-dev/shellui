using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AccordionContentTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion-content",
        DisplayName = "Accordion Content",
        Description = "Expandable content for AccordionItem (shadcn-style)",
        Category = ComponentCategory.Layout,
        FilePath = "AccordionContent.razor",
        Dependencies = new List<string> { "accordion-item" },
        IsAvailable = false,
        Tags = new List<string> { "layout", "accordion", "content", "body" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""grid grid-rows-[0fr] data-[state=open]:grid-rows-[1fr] transition-[grid-template-rows] duration-200 ease-[cubic-bezier(0.4,0,0.2,1)]"" data-state=""@(Parent?.EffectiveIsOpen == true ? ""open"" : ""closed"")"" @attributes=""AdditionalAttributes"">
    <div class=""overflow-hidden min-h-0"">
        <div class=""pb-4 pt-0 text-sm"">
            @ChildContent
        </div>
    </div>
</div>

@code {
    [CascadingParameter] public AccordionItem? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
";
}
