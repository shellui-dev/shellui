using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ShellUI.Components;
using ShellUI.Components.Models;

namespace ShellUI.Tests;

public class PackageBehaviorTests
{
    [Fact]
    public void Command_RunsTheItemActionThenRaisesCommandSelected()
    {
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var calls = new List<string>();
        var item = new CommandItem { Title = "Go home", Action = () => { calls.Add("action"); return Task.CompletedTask; } };

        var command = ctx.Render<Command>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Commands, new List<CommandItem> { item })
            .Add(c => c.CommandSelected, EventCallback.Factory.Create<CommandItem>(this, _ => calls.Add("selected"))));

        command.FindAll("button").First(b => b.TextContent.Contains("Go home")).Click();

        Assert.Equal(new[] { "action", "selected" }, calls);
    }

    [Fact]
    public void Popover_ClosesOnEscape()
    {
        using var ctx = new BunitContext();
        bool? open = null;

        var popover = ctx.Render<Popover>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.IsOpenChanged, EventCallback.Factory.Create<bool>(this, v => open = v))
            .Add(c => c.Trigger, "Open")
            .Add(c => c.ChildContent, "Body"));

        popover.Find("div.relative").KeyDown(new KeyboardEventArgs { Key = "Escape" });

        Assert.False(open);
        Assert.DoesNotContain("Body", popover.Markup);
    }

    [Fact]
    public void Dropdown_ClosesOnEscapeButNotOnOtherKeys()
    {
        using var ctx = new BunitContext();
        var changes = new List<bool>();

        var dropdown = ctx.Render<Dropdown>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.IsOpenChanged, EventCallback.Factory.Create<bool>(this, v => changes.Add(v)))
            .Add(c => c.Trigger, "Menu")
            .Add(c => c.ChildContent, "Item"));

        dropdown.Find("div.relative").KeyDown(new KeyboardEventArgs { Key = "a" });
        Assert.Empty(changes);

        dropdown.Find("div.relative").KeyDown(new KeyboardEventArgs { Key = "Escape" });
        Assert.Equal(new[] { false }, changes);
    }
}
