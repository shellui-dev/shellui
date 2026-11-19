using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ShellUI.Components;
using ShellUI.Components.Models;

namespace ShellUI.Tests;

public class ChatExtrasTests
{
    private static BunitContext Context()
    {
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        return ctx;
    }

    [Fact]
    public void Actions_CopyTheTextAndRegenerate()
    {
        using var ctx = Context();
        var regenerated = 0;

        var actions = ctx.Render<ChatMessageActions>(p => p
            .Add(c => c.CopyText, "The answer")
            .Add(c => c.OnRegenerate, EventCallback.Factory.Create(this, () => regenerated++))
            .AddChildContent("<span id=\"mine\"></span>"));

        Assert.Equal("The answer", actions.FindComponent<CopyButton>().Instance.Text);
        actions.Find("button[aria-label=Regenerate]").Click();
        Assert.Equal(1, regenerated);
        Assert.NotNull(actions.Find("#mine"));
    }

    [Fact]
    public void Actions_HideRegenerateWithoutAHandlerAndDisableIt()
    {
        using var ctx = Context();

        var plain = ctx.Render<ChatMessageActions>(p => p.Add(c => c.CopyText, "x"));
        Assert.Empty(plain.FindAll("button[aria-label=Regenerate]"));

        var disabled = ctx.Render<ChatMessageActions>(p => p
            .Add(c => c.OnRegenerate, EventCallback.Factory.Create(this, () => { }))
            .Add(c => c.Disabled, true));
        Assert.True(disabled.Find("button[aria-label=Regenerate]").HasAttribute("disabled"));
        Assert.Empty(disabled.FindComponents<CopyButton>());
    }

    [Fact]
    public void Input_HasNoAttachButtonUnlessAsked()
    {
        using var ctx = Context();

        var input = ctx.Render<ChatInput>();

        Assert.Empty(input.FindAll("input[type=file]"));
    }

    [Fact]
    public void Input_SendsAttachedFilesAndClearsThem()
    {
        using var ctx = Context();
        ChatSubmission? sent = null;
        string? submitted = null;

        var input = ctx.Render<ChatInput>(p => p
            .Add(c => c.AllowAttachments, true)
            .Add(c => c.OnSubmit, EventCallback.Factory.Create<string>(this, t => submitted = t))
            .Add(c => c.OnSend, EventCallback.Factory.Create<ChatSubmission>(this, s => sent = s)));

        input.FindComponent<InputFile>().UploadFiles(
            InputFileContent.CreateFromText("hello", "notes.txt"),
            InputFileContent.CreateFromText("world", "todo.txt"));
        Assert.Equal(2, input.FindAll("li").Count);

        input.Find("button[aria-label='Remove notes.txt']").Click();
        Assert.Single(input.FindAll("li"));

        input.Find("form").Submit();

        Assert.NotNull(sent);
        Assert.Equal("", sent.Text);
        Assert.Equal("todo.txt", Assert.Single(sent.Files).Name);
        Assert.Equal("", submitted);
        Assert.Empty(input.FindAll("li"));
    }

    [Fact]
    public void Input_RejectsFilesOverTheLimits()
    {
        using var ctx = Context();

        var input = ctx.Render<ChatInput>(p => p
            .Add(c => c.AllowAttachments, true)
            .Add(c => c.MaxFiles, 1)
            .Add(c => c.MaxFileSize, 8));

        input.FindComponent<InputFile>().UploadFiles(InputFileContent.CreateFromText("0123456789", "big.txt"));
        Assert.Empty(input.FindAll("li"));
        Assert.Contains("big.txt is larger than 8 B", input.Find("[role=alert]").TextContent);

        input.FindComponent<InputFile>().UploadFiles(
            InputFileContent.CreateFromText("a", "a.txt"),
            InputFileContent.CreateFromText("b", "b.txt"));
        Assert.Single(input.FindAll("li"));
        Assert.Contains("up to 1 files", input.Find("[role=alert]").TextContent);
    }

    [Fact]
    public void Input_StillSendsTextOnlyMessagesThroughBothCallbacks()
    {
        using var ctx = Context();
        ChatSubmission? sent = null;
        string? submitted = null;

        var input = ctx.Render<ChatInput>(p => p
            .Add(c => c.Value, "  Hi there ")
            .Add(c => c.OnSubmit, EventCallback.Factory.Create<string>(this, t => submitted = t))
            .Add(c => c.OnSend, EventCallback.Factory.Create<ChatSubmission>(this, s => sent = s)));

        input.Find("form").Submit();

        Assert.Equal("Hi there", submitted);
        Assert.Equal("Hi there", sent!.Text);
        Assert.Empty(sent.Files);
    }
}
