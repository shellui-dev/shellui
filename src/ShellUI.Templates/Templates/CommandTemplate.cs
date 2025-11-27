using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class CommandTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "command",
        DisplayName = "Command",
        Description = "Command palette component for quick actions",
        Category = ComponentCategory.Overlay,
        FilePath = "Command.razor",
        Dependencies = new List<string> { "dialog" }
    };

    public static string Content => @"
@using Microsoft.JSInterop

@if (IsOpen)
{
    <div class=""fixed inset-0 z-50 bg-background/80 backdrop-blur-sm"" @onclick=""CloseCommand""></div>
    <div class=""fixed left-[50%] top-[50%] z-50 grid w-full max-w-lg translate-x-[-50%] translate-y-[-50%] gap-4 border bg-background p-0 shadow-lg duration-200 sm:rounded-lg"">
        <!-- Search Input -->
        <div class=""flex items-center border-b px-3"">
            <svg class=""mr-2 h-4 w-4 shrink-0 opacity-50"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
                <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"" />
            </svg>
            <input
                @ref=""_searchInput""
                @bind-value=""_searchQuery""
                @bind-value:event=""oninput""
                @onkeydown=""OnKeyDown""
                type=""text""
                placeholder=""@Placeholder""
                class=""flex h-11 w-full rounded-md bg-transparent py-3 text-sm outline-none placeholder:text-muted-foreground disabled:cursor-not-allowed disabled:opacity-50"" />
        </div>

        <!-- Results -->
        <div class=""max-h-[300px] overflow-y-auto p-2"">
            @if (_filteredCommands.Any())
            {
                @foreach (var command in _filteredCommands)
                {
                    <button
                        @onclick=""() => SelectCommand(command)""
                        class=""flex w-full items-center rounded-md px-2 py-1.5 text-sm hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground focus:outline-none"">
                        @if (!string.IsNullOrEmpty(command.Icon))
                        {
                            <div class=""mr-3 flex h-4 w-4 items-center justify-center"">
                                @command.Icon
                            </div>
                        }
                        <div class=""flex-1 text-left"">
                            <div class=""font-medium"">@command.Title</div>
                            @if (!string.IsNullOrEmpty(command.Description))
                            {
                                <div class=""text-xs text-muted-foreground"">@command.Description</div>
                            }
                        </div>
                        @if (!string.IsNullOrEmpty(command.Shortcut))
                        {
                            <div class=""ml-3 flex items-center gap-0.5"">
                                @foreach (var key in command.Shortcut.Split('+'))
                                {
                                    <kbd class=""pointer-events-none inline-flex h-5 select-none items-center gap-1 rounded border bg-muted px-1.5 font-mono text-[10px] font-medium text-muted-foreground opacity-100"">
                                        <span class=""text-xs"">@key</span>
                                    </kbd>
                                }
                            </div>
                        }
                    </button>
                }
            }
            else
            {
                <div class=""p-6 text-center text-sm"">
                    No results found.
                </div>
            }
        </div>
    </div>
}

@code {
    [Parameter]
    public bool IsOpen { get; set; }

    [Parameter]
    public string Placeholder { get; set; } = ""Type a command or search..."";

    [Parameter]
    public List<CommandItem> Commands { get; set; } = new();

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    [Parameter]
    public EventCallback<CommandItem> CommandSelected { get; set; }

    private string _searchQuery = """";
    private List<CommandItem> _filteredCommands = new();
    private ElementReference _searchInput;

    protected override void OnParametersSet()
    {
        UpdateFilteredCommands();
    }

    private void UpdateFilteredCommands()
    {
        if (string.IsNullOrWhiteSpace(_searchQuery))
        {
            _filteredCommands = Commands.ToList();
        }
        else
        {
            _filteredCommands = Commands
                .Where(c =>
                    c.Title.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    (c.Description?.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) == true))
                .ToList();
        }
    }

    private async Task SelectCommand(CommandItem command)
    {
        await CommandSelected.InvokeAsync(command);
        await CloseCommand();
    }

    private async Task CloseCommand()
    {
        IsOpen = false;
        _searchQuery = """";
        await IsOpenChanged.InvokeAsync(IsOpen);
    }

    private void OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == ""Escape"")
        {
            CloseCommand();
        }
        else if (e.Key == ""Enter"" && _filteredCommands.Any())
        {
            SelectCommand(_filteredCommands.First());
        }
        else if (e.Key == ""ArrowDown"" && _filteredCommands.Count > 1)
        {
            // Could implement keyboard navigation here
        }
        else if (e.Key == ""ArrowUp"" && _filteredCommands.Count > 1)
        {
            // Could implement keyboard navigation here
        }
    }
}

public class CommandItem
{
    public string Title { get; set; } = """";
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Shortcut { get; set; }
    public Action? Action { get; set; }
}";
}
