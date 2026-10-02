using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class MenubarMenuTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "menubar-menu",
        DisplayName = "Menubar Menu",
        Description = "Submenu container for the compositional Menubar pattern",
        Category = ComponentCategory.Navigation,
        FilePath = "MenubarMenu.razor",
        IsAvailable = false,
        Tags = new List<string> { "navigation", "menubar", "menu" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<CascadingValue Value=""this"" IsFixed=""true"">
    <div class=""@Shell.Cn(""relative"", Class)"" @attributes=""AdditionalAttributes"">
        @ChildContent
    </div>
</CascadingValue>

@if (IsOpen)
{
    <div class=""fixed inset-0 z-40"" @onclick=""Close""></div>
}

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    public bool IsOpen { get; private set; }

    public void Toggle()
    {
        IsOpen = !IsOpen;
        StateHasChanged();
    }

    public void Open()
    {
        IsOpen = true;
        StateHasChanged();
    }

    public void Close()
    {
        IsOpen = false;
        StateHasChanged();
    }
}
";
}
