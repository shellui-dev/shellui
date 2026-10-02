using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-trigger",
        DisplayName = "Menubar Trigger",
        Description = "Trigger button for the compositional Menubar pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menubar", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<button type=""button""
        @onclick=""Toggle""
        class=""@Shell.Cn(""flex cursor-default select-none items-center rounded-sm px-3 py-1.5 text-sm font-medium outline-none hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground"", Class)""
        data-state=""@(Parent?.IsOpen == true ? ""open"" : ""closed"")""
        @attributes=""AdditionalAttributes"">
    @ChildContent
</button>

@code {
    [CascadingParameter] private MenubarMenu? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private void Toggle() => Parent?.Toggle();
}
";
}
