using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class CommandInputTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-input",
        DisplayName = "Command Input",
        Description = "Search box for a compositional Command",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandInput.razor",
        IsAvailable = false,
        Tags = new List<string> { "command", "input" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI

<div class=""flex items-center border-b border-border px-3"">
    <svg class=""mr-2 h-4 w-4 shrink-0 opacity-50"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
        <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M21 21l-4.35-4.35M17 11a6 6 0 11-12 0 6 6 0 0112 0z"" />
    </svg>
    <input @ref=""_input""
           type=""text""
           role=""combobox""
           aria-expanded=""true""
           autocomplete=""off""
           spellcheck=""false""
           value=""@_value""
           @oninput=""OnInput""
           @onkeydown=""OnKeyDownAsync""
           placeholder=""@Placeholder""
           class=""@Shell.Cn(""flex h-11 w-full rounded-md bg-transparent py-3 text-sm outline-none placeholder:text-muted-foreground disabled:cursor-not-allowed disabled:opacity-50"", Class)""
           @attributes=""AdditionalAttributes"" />
</div>

@code {
    [CascadingParameter] private Command? Parent { get; set; }
    [Parameter] public string Placeholder { get; set; } = ""Type a command or search..."";
    [Parameter] public bool AutoFocus { get; set; } = true;
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private ElementReference _input;
    private string _value = """";

    private void OnInput(ChangeEventArgs e)
    {
        _value = e.Value?.ToString() ?? """";
        Parent?.SetSearch(_value);
    }

    private async Task OnKeyDownAsync(KeyboardEventArgs e)
    {
        if (Parent == null) return;
        switch (e.Key)
        {
            case ""ArrowDown"": Parent.MoveActive(1); break;
            case ""ArrowUp"": Parent.MoveActive(-1); break;
            case ""Home"": Parent.MoveActive(-1, toEnd: true); break;
            case ""End"": Parent.MoveActive(1, toEnd: true); break;
            case ""Enter"": await Parent.SelectActiveAsync(); break;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && AutoFocus)
        {
            try { await _input.FocusAsync(); } catch (InvalidOperationException) { }
        }
    }
}
";
}
