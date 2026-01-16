using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class TabsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "tabs",
        DisplayName = "Tabs",
        Description = "Tabbed navigation component",
        Category = ComponentCategory.Navigation,

        FilePath = "Tabs.razor",
        Dependencies = new List<string> { "tab-models" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using ShellUI.Components.Models

<div class=""@Shell.Cn(""w-full"", Class)"" @attributes=""AdditionalAttributes"">
    <div class=""inline-flex h-10 items-center justify-center rounded-md bg-muted p-1 text-muted-foreground"">
        @if (Items != null)
        {
            foreach (var item in Items)
            {
                <button type=""button""
                        @onclick=""() => SetActiveTab(item.Id)""
                        class=""@Shell.Cn(""inline-flex items-center justify-center whitespace-nowrap rounded-sm px-3 py-1.5 text-sm font-medium ring-offset-background transition-all focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50"", ActiveTab == item.Id ? ""bg-background text-foreground shadow-sm"" : """")"">
                    @if (!string.IsNullOrEmpty(item.Icon))
                    {
                        <span class=""mr-2"">@((MarkupString)item.Icon)</span>
                    }
                    @item.Label
                </button>
            }
        }
        @TabHeaders
    </div>
    <div class=""mt-2 ring-offset-background focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2"">
        @if (Items != null)
        {
            var activeItem = Items.FirstOrDefault(x => x.Id == ActiveTab);
            if (activeItem != null && activeItem.Content != null)
            {
                @activeItem.Content
            }
        }
        @TabContent
    </div>
</div>

@code {
    [Parameter] public IEnumerable<TabItem>? Items { get; set; }
    [Parameter] public string ActiveTab { get; set; } = """";
    [Parameter] public EventCallback<string> ActiveTabChanged { get; set; }
    [Parameter] public RenderFragment? TabHeaders { get; set; }
    [Parameter] public RenderFragment? TabContent { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task SetActiveTab(string id)
    {
        ActiveTab = id;
        await ActiveTabChanged.InvokeAsync(id);
    }
}
";
}

