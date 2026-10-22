using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
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

        command.Find("[role=option]").Click();

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

    [Fact]
    public void DialogContent_TrapsFocusWhileOpenAndReleasesItOnClose()
    {
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var dialog = ctx.Render<Dialog>(p => p
            .Add(c => c.Open, true)
            .AddChildContent<DialogContent>(c => c.AddChildContent("Body")));

        var trap = ctx.JSInterop.VerifyInvoke("ShellUI.trapFocus");
        ctx.JSInterop.VerifyNotInvoke("ShellUI.releaseFocus");

        dialog.Render(p => p.Add(c => c.Open, false));

        var release = ctx.JSInterop.VerifyInvoke("ShellUI.releaseFocus");
        Assert.Equal(trap.Arguments[0], release.Arguments[0]);
    }

    [Fact]
    public void SheetContent_TrapsFocusWhileOpen()
    {
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        ctx.Render<Sheet>(p => p
            .Add(c => c.Open, true)
            .AddChildContent<SheetContent>(c => c.AddChildContent("Body")));

        ctx.JSInterop.VerifyInvoke("ShellUI.trapFocus");
    }

    [Fact]
    public void LegacySheet_TrapsFocusAndReleasesItWhenEscapeCloses()
    {
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        bool? open = null;

        var sheet = ctx.Render<Sheet>(p => p
            .Add(c => c.Open, true)
            .Add(c => c.Title, "Settings")
            .Add(c => c.OpenChanged, EventCallback.Factory.Create<bool>(this, v => open = v))
            .AddChildContent("Body"));

        ctx.JSInterop.VerifyInvoke("ShellUI.trapFocus");

        sheet.Find("[role=dialog]").KeyDown(new KeyboardEventArgs { Key = "Escape" });

        Assert.False(open);
        ctx.JSInterop.VerifyInvoke("ShellUI.releaseFocus");
    }

    [Fact]
    public void RadioGroup_SelectsTheClickedItemButNotADisabledOne()
    {
        using var ctx = new BunitContext();
        var changes = new List<string>();

        var group = ctx.Render<RadioGroup>(p => p
            .Add(c => c.ValueChanged, EventCallback.Factory.Create<string>(this, v => changes.Add(v)))
            .AddChildContent<RadioGroupItem>(i => i.Add(c => c.Value, "a").AddChildContent("A"))
            .AddChildContent<RadioGroupItem>(i => i.Add(c => c.Value, "b").Add(c => c.Disabled, true).AddChildContent("B")));

        var buttons = group.FindAll("button");
        buttons[1].Click();
        buttons[0].Click();

        Assert.Equal(new[] { "a" }, changes);
    }

    [Fact]
    public void Pagination_RaisesBothCallbacksAndIgnoresTheCurrentPage()
    {
        using var ctx = new BunitContext();
        var bound = new List<int>();
        var legacy = new List<int>();

        var pagination = ctx.Render<Pagination>(p => p
            .Add(c => c.CurrentPage, 2)
            .Add(c => c.TotalPages, 3)
            .Add(c => c.CurrentPageChanged, EventCallback.Factory.Create<int>(this, v => bound.Add(v)))
            .Add(c => c.OnPageChange, EventCallback.Factory.Create<int>(this, v => legacy.Add(v))));

        pagination.FindAll("button").Single(b => b.TextContent.Trim() == "2").Click();
        pagination.FindAll("button").Single(b => b.TextContent.Trim() == "3").Click();

        Assert.Equal(new[] { 3 }, bound);
        Assert.Equal(new[] { 3 }, legacy);
    }

    [Fact]
    public void DatePicker_PlacesTheFirstOfTheMonthUnderItsWeekday()
    {
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        // 1 October 2026 is a Thursday, so four blank cells precede it.
        var picker = ctx.Render<DatePicker>(p => p.Add(c => c.Value, new DateTime(2026, 10, 15)));

        picker.Find("button").Click();

        var grid = picker.FindAll("div.grid.grid-cols-7")[1];
        var cells = grid.Children;
        Assert.Equal(4, cells.TakeWhile(c => c.TagName == "DIV").Count());
        Assert.Equal("1", cells[4].TextContent.Trim());
    }

    [Theory]
    [InlineData(null, null, "max-height: 400px;")]
    [InlineData("200px", null, "height: 200px; max-height: none;")]
    [InlineData(null, "300px", "max-height: 300px;")]
    public void ScrollArea_SizesTheScrollingElement(string? height, string? maxHeight, string expected)
    {
        using var ctx = new BunitContext();

        var area = ctx.Render<ScrollArea>(p => p
            .Add(c => c.Height, height)
            .Add(c => c.MaxHeight, maxHeight)
            .AddChildContent("Body"));

        Assert.Equal(expected, area.Find("div.overflow-auto").GetAttribute("style"));
    }

    [Fact]
    public void CompositionalStepper_ShowsTheActiveStepAfterNext()
    {
        using var ctx = new BunitContext();
        var changes = new List<int>();

        var stepper = ctx.Render<Stepper>(p => p
            .Add(c => c.TotalSteps, 2)
            .Add(c => c.CurrentStepChanged, EventCallback.Factory.Create<int>(this, v => changes.Add(v)))
            .AddChildContent<StepperList>(l => l
                .AddChildContent<StepperStep>(s => s.Add(c => c.Value, "0").Add(c => c.Title, "Account"))
                .AddChildContent<StepperStep>(s => s.Add(c => c.Value, "1").Add(c => c.Title, "Profile")))
            .AddChildContent<StepperContent>(c => c.Add(x => x.Value, "0").AddChildContent("First"))
            .AddChildContent<StepperContent>(c => c.Add(x => x.Value, "1").AddChildContent("Second")));

        Assert.Contains("First", stepper.Markup);
        stepper.FindAll("button").Single(b => b.TextContent.Trim() == "Next").Click();

        Assert.Equal(new[] { 1 }, changes);
        Assert.Contains("Second", stepper.Markup);
        Assert.DoesNotContain("First", stepper.Markup);
    }

    [Fact]
    public void TimePicker_ShowsASetValue()
    {
        using var ctx = new BunitContext();

        var picker = ctx.Render<TimePicker>(p => p.Add(c => c.Value, new TimeSpan(9, 5, 0)));

        Assert.Contains("09:05", picker.Markup);
    }

    [Fact]
    public async Task Sonner_ClearFadesEveryToastThenRemovesIt()
    {
        using var ctx = new BunitContext();
        ctx.Services.AddShellUISonner();
        var service = ctx.Services.GetRequiredService<ShellUI.Components.Services.SonnerService>();
        var sonner = ctx.Render<Sonner>();

        await sonner.InvokeAsync(() => service.Show("Saved", duration: TimeSpan.Zero));
        await sonner.InvokeAsync(() => service.Error("Failed", duration: TimeSpan.Zero));
        await sonner.InvokeAsync(service.Clear);

        Assert.All(service.Toasts, t => Assert.True(t.Dismissing));
        Assert.Equal(2, sonner.FindAll(".opacity-0").Count);
        sonner.WaitForAssertion(() => Assert.Empty(service.Toasts));
        Assert.DoesNotContain("Saved", sonner.Markup);
    }

    [Fact]
    public void ContextMenuOption_Destructive_ReplacesTheAccentColours()
    {
        using var ctx = new BunitContext();
        var option = ctx.Render<ContextMenuOption>(p => p.Add(c => c.Destructive, true).Add(c => c.ChildContent, "Delete"));

        var classes = option.Find("button").ClassList;
        Assert.Contains("text-destructive", classes);
        Assert.Contains("hover:bg-destructive/10", classes);
        Assert.DoesNotContain("hover:bg-accent", classes);
    }

    [Fact]
    public void ContextMenu_DestructiveItem_IsRed()
    {
        using var ctx = new BunitContext();
        var menu = ctx.Render<ContextMenu>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Items, new List<ContextMenuItem> { new() { Label = "Edit" }, new() { Label = "Delete", Destructive = true } }));

        var buttons = menu.FindAll("button");
        Assert.DoesNotContain("text-destructive", buttons[0].ClassList);
        Assert.Contains("text-destructive", buttons[1].ClassList);
    }
}
