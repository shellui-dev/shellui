using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class NavigationMenuItemTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "navigation-menu-item",
        DisplayName = "Navigation Menu Item",
        Description = "Individual navigation menu item",
        Category = ComponentCategory.Navigation,
        FilePath = "NavigationMenuItem.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menu", "nav", "item" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

@if (!string.IsNullOrEmpty(Title))
{
    <div class=""@Shell.Cn(""relative"", ClassName, Class)"" @attributes=""AdditionalAttributes"">
        <button type=""button""
                @onclick=""Toggle""
                disabled=""@Disabled""
                class=""group inline-flex h-10 w-max items-center justify-center rounded-md bg-background px-4 py-2 text-sm font-medium transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground focus:outline-none disabled:pointer-events-none disabled:opacity-50"">
            @Title
            @if (HasContent)
            {
                <svg class=""@Shell.Cn(""relative top-[1px] ml-1 h-3 w-3 transition duration-200"", IsOpen ? ""rotate-180"" : """")"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                    <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
                </svg>
            }
        </button>

        @if (IsOpen && HasContent)
        {
            <div class=""absolute left-0 top-full mt-1 w-56 rounded-md border border-border bg-popover p-1 shadow-lg animate-in fade-in-0 zoom-in-95"">
                @ChildContent
            </div>
        }
    </div>
}
else if (!string.IsNullOrEmpty(Href))
{
    <a href=""@Href""
       aria-current=""@(IsActive ? ""page"" : null)""
       class=""@Shell.Cn(""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring h-9 px-4 py-2"", IsActive ? ""bg-accent text-accent-foreground"" : ""hover:bg-accent hover:text-accent-foreground"", ClassName, Class)""
       @attributes=""AdditionalAttributes"">
        @ChildContent
    </a>
}
else
{
    <button type=""button""
            @onclick=""OnClick""
            disabled=""@Disabled""
            class=""@Shell.Cn(""inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring h-9 px-4 py-2"", Disabled ? ""cursor-not-allowed opacity-50"" : """", IsActive ? ""bg-accent text-accent-foreground"" : ""hover:bg-accent hover:text-accent-foreground"", ClassName, Class)""
            @attributes=""AdditionalAttributes"">
        @ChildContent
    </button>
}

@code {
    // Set Title for a dropdown item: Title is the trigger and ChildContent the dropdown.
    // Leave it empty for a plain item: ChildContent is the label, rendered as a link when Href is set.
    [Parameter] public string Title { get; set; } = """";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool HasContent { get; set; } = true;
    [Parameter] public string? Href { get; set; }
    [Parameter] public bool IsActive { get; set; }
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public string? Class { get; set; }
    // Deprecated: use Class.
    [Parameter] public string ClassName { get; set; } = """";
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private bool IsOpen { get; set; }

    private void Toggle() => IsOpen = !IsOpen;
}
";
}


