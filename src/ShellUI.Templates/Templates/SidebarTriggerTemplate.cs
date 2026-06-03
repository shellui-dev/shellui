using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-trigger",
        DisplayName = "Sidebar Trigger",
        Description = "Toggle button to open/close the sidebar",
        Category = ComponentCategory.Layout,
        FilePath = "SidebarTrigger.razor",
        IsAvailable = false
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<button type=""button""
        data-sidebar=""trigger""
        class=""@Shell.Cn(
            ""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 hover:bg-accent hover:text-accent-foreground h-7 w-7"",
            Class)""
        @onclick=""HandleClick""
        @attributes=""AdditionalAttributes"">
    <i class=""fa-solid fa-bars-staggered text-sm""></i>
    <span class=""sr-only"">Toggle Sidebar</span>
</button>

@code {
    [CascadingParameter] public SidebarProvider? SidebarProvider { get; set; }

    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task HandleClick(MouseEventArgs args)
    {
        await OnClick.InvokeAsync(args);
        SidebarProvider?.Toggle();
    }
}
";
}
