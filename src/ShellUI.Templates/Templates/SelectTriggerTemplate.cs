using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SelectTriggerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "select-trigger",
        DisplayName = "Select Trigger",
        Description = "Trigger button for custom Select",
        Category = ComponentCategory.Form,
        FilePath = "SelectTrigger.razor",
        IsAvailable = false,
        Tags = new List<string> { "form", "select", "trigger" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<button type=""button""
        @onclick=""ToggleAsync""
        disabled=""@(Parent?.Disabled == true)""
        aria-haspopup=""listbox""
        aria-expanded=""@((Parent?.IsOpen == true).ToString().ToLowerInvariant())""
        class=""@Shell.Cn(""flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 pr-8 text-sm ring-offset-background placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50"", Parent?.ClassName, Parent?.Class)""
        @attributes=""AdditionalAttributes"">
    @ChildContent
    <svg class=""h-4 w-4 opacity-50"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
    </svg>
</button>

@code {
    [CascadingParameter] public Select? Parent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private Task ToggleAsync()
    {
        if (Parent != null && Parent.UseCustomSelect)
        {
            Parent.IsOpen = !Parent.IsOpen;
            Parent.InvokeStateHasChanged();
        }
        return Task.CompletedTask;
    }
}
";
}
