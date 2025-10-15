using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class ComboboxTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "combobox",
        DisplayName = "Combobox",
        Description = "Searchable dropdown combobox component",
        Category = ComponentCategory.Form,
        FilePath = "Combobox.razor",
        Version = "0.1.0",
        Tags = new List<string> { "form", "select", "dropdown", "search", "autocomplete" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""@(""relative "" + ClassName)"" @attributes=""AdditionalAttributes"">
    <button type=""button""
            @onclick=""ToggleOpen""
            disabled=""@Disabled""
            class=""@(""flex h-10 w-full items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 "" + (Disabled ? ""opacity-50 cursor-not-allowed"" : """"))"">
        <span class=""@(string.IsNullOrEmpty(SelectedValue) ? ""text-muted-foreground"" : """")"">
            @(string.IsNullOrEmpty(SelectedValue) ? Placeholder : SelectedValue)
        </span>
        <svg class=""h-4 w-4 opacity-50"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
        </svg>
    </button>
    
    @if (_isOpen)
    {
        <div class=""absolute z-50 w-full mt-1 rounded-md border bg-popover text-popover-foreground shadow-md"">
            <div class=""p-2"">
                <input type=""text""
                       @bind=""_searchQuery""
                       @bind:event=""oninput""
                       placeholder=""Search...""
                       class=""flex h-9 w-full rounded-md border border-input bg-background px-3 py-1 text-sm shadow-sm focus:outline-none focus:ring-1 focus:ring-ring"" />
            </div>
            <div class=""max-h-60 overflow-auto p-1"">
                @foreach (var item in FilteredOptions)
                {
                    <div @onclick=""@(() => SelectOption(item))""
                         class=""@(""relative flex cursor-pointer select-none items-center rounded-sm px-2 py-1.5 text-sm outline-none hover:bg-accent hover:text-accent-foreground "" + (item == SelectedValue ? ""bg-accent"" : """"))"">
                        @item
                    </div>
                }
                @if (!FilteredOptions.Any())
                {
                    <div class=""py-6 text-center text-sm text-muted-foreground"">No results found.</div>
                }
            </div>
        </div>
    }
</div>

@code {
    [Parameter]
    public string SelectedValue { get; set; } = """";
    
    [Parameter]
    public EventCallback<string> SelectedValueChanged { get; set; }
    
    [Parameter]
    public List<string> Options { get; set; } = new();
    
    [Parameter]
    public string Placeholder { get; set; } = ""Select option..."";
    
    [Parameter]
    public bool Disabled { get; set; }
    
    [Parameter]
    public string ClassName { get; set; } = """";
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private bool _isOpen;
    private string _searchQuery = """";
    
    private IEnumerable<string> FilteredOptions => 
        string.IsNullOrWhiteSpace(_searchQuery) 
            ? Options 
            : Options.Where(o => o.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase));
    
    private void ToggleOpen() => _isOpen = !_isOpen;
    
    private async Task SelectOption(string option)
    {
        SelectedValue = option;
        _isOpen = false;
        _searchQuery = """";
        await SelectedValueChanged.InvokeAsync(option);
    }
}
";
}

