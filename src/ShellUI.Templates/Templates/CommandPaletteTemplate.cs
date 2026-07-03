using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CommandPaletteTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command-palette",
        DisplayName = "CommandPalette",
        Description = "Cmd+K palette wrapper — binds a global hotkey to open a Command",
        Category = ComponentCategory.Overlay,
        FilePath = "CommandPalette.razor",
        Dependencies = new List<string> { "command" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using Microsoft.JSInterop
@implements IAsyncDisposable
@inject IJSRuntime JS

<Command IsOpen=""_isOpen""
         IsOpenChanged=""OnIsOpenChanged""
         Commands=""Commands""
         CommandSelected=""OnCommandSelected""
         Placeholder=""@Placeholder"" />

@code {
    [Parameter] public List<CommandItem> Commands { get; set; } = new();
    [Parameter] public EventCallback<CommandItem> CommandSelected { get; set; }
    [Parameter] public string Placeholder { get; set; } = ""Type a command or search..."";
    [Parameter] public string HotkeyChar { get; set; } = ""k"";
    [Parameter] public bool UseCtrl { get; set; } = true;
    [Parameter] public bool UseMeta { get; set; } = true;
    [Parameter] public bool UseShift { get; set; }
    [Parameter] public bool UseAlt { get; set; }

    private bool _isOpen;
    private IJSObjectReference? _module;
    private DotNetObjectReference<CommandPalette>? _selfRef;
    private readonly string _handle = Guid.NewGuid().ToString(""N"");

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        _selfRef = DotNetObjectReference.Create(this);
        _module = await JS.InvokeAsync<IJSObjectReference>(""import"", ""./_content/ShellUI.Components/shellui.js"");
        await _module.InvokeVoidAsync(""registerShortcut"", _handle, HotkeyChar, UseCtrl, UseMeta, UseShift, UseAlt, _selfRef);
    }

    [JSInvokable]
    public Task OnShortcut()
    {
        _isOpen = !_isOpen;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task OnIsOpenChanged(bool open)
    {
        _isOpen = open;
        return Task.CompletedTask;
    }

    private async Task OnCommandSelected(CommandItem item)
    {
        await CommandSelected.InvokeAsync(item);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync(""unregisterShortcut"", _handle);
                await _module.DisposeAsync();
            }
        }
        catch { }
        _selfRef?.Dispose();
    }
}
";
}
