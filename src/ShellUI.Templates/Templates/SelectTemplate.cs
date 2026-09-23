using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SelectTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "select",
        DisplayName = "Select",
        Description = "Dropdown select input component",
        Category = ComponentCategory.Form,

        FilePath = "Select.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""relative"">
    <select 
        value=""@Value""
        disabled=""@Disabled""
        @onchange=""HandleChange""
        class=""flex h-10 w-full items-center appearance-none rounded-md border border-input bg-background px-3 pr-8 py-2 text-sm ring-offset-background placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50 @ClassName""
        @attributes=""AdditionalAttributes"">
        @ChildContent
    </select>
    <svg class=""pointer-events-none absolute right-3 top-1/2 h-4 w-4 -translate-y-1/2 text-muted-foreground""
         xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M19 9l-7 7-7-7"" />
    </svg>
</div>

@code {
    [Parameter]
    public string? Value { get; set; }

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string ClassName { get; set; } = """";

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private async Task HandleChange(ChangeEventArgs e)
    {
        Value = e.Value?.ToString();
        await ValueChanged.InvokeAsync(Value);
    }
}
";
}


