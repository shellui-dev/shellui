using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class AccordionTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "accordion-trigger",
        DisplayName = "Accordion Trigger",
        Description = "Trigger button for AccordionItem (shadcn-style)",
        Category = ComponentCategory.Layout,
        FilePath = "AccordionTrigger.razor",
        Dependencies = new List<string> { "accordion-item" },
        IsAvailable = false,
        Tags = new List<string> { "layout", "accordion", "trigger", "toggle" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<button type=""button""
        @onclick=""Toggle""
        disabled=""@(Parent?.Disabled ?? false)""
        class=""flex w-full flex-1 items-center justify-between py-4 font-medium transition-all hover:underline disabled:pointer-events-none disabled:opacity-50 [&[data-state=open]>svg]:rotate-180""
        data-state=""@(Parent?.EffectiveIsOpen == true ? ""open"" : ""closed"")""
        @attributes=""AdditionalAttributes"">
    @ChildContent
    <svg class=""h-4 w-4 shrink-0 transition-transform duration-200"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
    </svg>
</button>

@code {
    [CascadingParameter] public AccordionItem? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task Toggle()
    {
        if (Parent != null)
            await Parent.ToggleAsync();
    }
}
";
}
