using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ContextMenuTemplate
{
    public static readonly ComponentMetadata Metadata = new()
    {
        Name = "ContextMenu",
        Description = "A context menu component that appears on right-click",
        Category = ComponentCategory.Navigation,
        Tags = new[] { "menu", "context", "right-click", "dropdown" },
        Dependencies = new[] { "ContextMenuItem" }
    };

    public const string Content = """
@using Microsoft.JSInterop
@using BlazorInteractiveServer.Components.Models

<div class="relative inline-block text-left" @oncontextmenu="OnContextMenu" @oncontextmenu:preventDefault="true">
    @ChildContent
    
    @if (IsOpen)
    {
        <div class="absolute right-0 z-50 mt-2 w-56 origin-top-right rounded-md border border-border bg-popover p-1 text-popover-foreground shadow-md">
            @foreach (var item in Items)
            {
                @if (item.IsSeparator)
                {
                    <div class="my-1 h-px bg-border"></div>
                }
                else
                {
                    <button class="relative flex w-full cursor-default select-none items-center rounded-sm px-2 py-1.5 text-sm outline-none hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground disabled:pointer-events-none disabled:opacity-50"
                            disabled="@item.Disabled"
                            @onclick="() => SelectItem(item)">
                        @if (!string.IsNullOrEmpty(item.Icon))
                        {
                            <span class="mr-2 h-4 w-4">@((MarkupString)item.Icon)</span>
                        }
                        <span>@item.Label</span>
                        @if (!string.IsNullOrEmpty(item.Shortcut))
                        {
                            <span class="ml-auto text-xs text-muted-foreground"> @item.Shortcut</span>
                        }
                    </button>
                }
            }
        </div>
    }
</div>

@code {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public List<ContextMenuItem> Items { get; set; } = new();
    [Parameter] public EventCallback<ContextMenuItem> OnItemSelected { get; set; }
    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

    private async Task OnContextMenu(MouseEventArgs e)
    {
        IsOpen = true;
        await IsOpenChanged.InvokeAsync(IsOpen);
        StateHasChanged();
    }

    private async Task SelectItem(ContextMenuItem item)
    {
        if (!item.Disabled)
        {
            await OnItemSelected.InvokeAsync(item);
            IsOpen = false;
            await IsOpenChanged.InvokeAsync(IsOpen);
        }
    }
}
""";
}
