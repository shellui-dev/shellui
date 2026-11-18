using Bunit;
using ShellUI.Components;

namespace ShellUI.Tests;

public class MarkdownTests
{
    private static IRenderedComponent<Markdown> Render(BunitContext ctx, string content)
    {
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        return ctx.Render<Markdown>(p => p.Add(c => c.Content, content));
    }

    [Fact]
    public void StylesHeadingsListsAndInlineCode()
    {
        using var ctx = new BunitContext();

        var md = Render(ctx, "# Title\n\nSome **bold** and `code`.\n\n- one\n- two\n\n1. first");

        Assert.Contains("text-2xl", md.Find("h1").ClassName);
        Assert.Equal("bold", md.Find("strong").TextContent);
        Assert.Contains("font-mono", md.Find("p code").ClassName);
        Assert.Contains("list-disc", md.Find("ul").ClassName);
        Assert.Contains("list-decimal", md.Find("ol").ClassName);
    }

    [Fact]
    public void ShowsRawHtmlAsText()
    {
        using var ctx = new BunitContext();

        var md = Render(ctx, "<script>alert(1)</script>\n\nHi <img src=x onerror=alert(1)> there");

        Assert.Empty(md.FindAll("script"));
        Assert.Empty(md.FindAll("img"));
        Assert.Contains("<script>alert(1)</script>", md.Find("p").TextContent);
    }

    [Theory]
    [InlineData("[click](javascript:alert(1))")]
    [InlineData("[click](JavaScript:alert(1))")]
    [InlineData("[click](data:text/html;base64,PHNjcmlwdD4=)")]
    [InlineData("<javascript:alert(1)>")]
    public void NeutralisesUnsafeLinks(string content)
    {
        using var ctx = new BunitContext();

        var md = Render(ctx, content);

        foreach (var link in md.FindAll("a"))
            Assert.DoesNotContain(":", link.GetAttribute("href") ?? "");
        Assert.DoesNotContain("javascript:", md.Markup, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void KeepsSafeLinks()
    {
        using var ctx = new BunitContext();

        var md = Render(ctx, "[docs](https://shellui.dev) [home](/) [mail](mailto:hi@shellui.dev)");

        Assert.Equal(new[] { "https://shellui.dev", "/", "mailto:hi@shellui.dev" }, md.FindAll("a").Select(a => a.GetAttribute("href")));
        Assert.All(md.FindAll("a"), a => Assert.Equal("noopener noreferrer", a.GetAttribute("rel")));
    }

    [Fact]
    public void IgnoresTheAttributeSyntax()
    {
        using var ctx = new BunitContext();

        var md = Render(ctx, "# Hi {onclick=alert(1)}\n\n[x](/){onmouseover=alert(1)}");

        // The braces stay as text instead of becoming attributes.
        Assert.Empty(md.FindAll("[onclick]"));
        Assert.Empty(md.FindAll("[onmouseover]"));
        Assert.Contains("{onclick=alert(1)}", md.Find("h1").TextContent);
    }

    [Fact]
    public void RendersFencedCodeAsACodeBlockWithACopyButton()
    {
        using var ctx = new BunitContext();

        var md = Render(ctx, "Run this:\n\n```html\n<b>bold</b>\n```\n\nThen this.");

        var block = md.FindComponent<CodeBlock>();
        Assert.Equal("html", block.Instance.Language);
        Assert.Equal("<b>bold</b>", block.Find("pre code").TextContent);
        Assert.Empty(block.FindAll("pre b"));
        Assert.Single(md.FindComponents<CopyButton>());
        Assert.Equal(2, md.FindAll("p").Count);
    }

    [Fact]
    public void RendersTablesAndTaskLists()
    {
        using var ctx = new BunitContext();

        var md = Render(ctx, "| A | B |\n|---|---|\n| 1 | 2 |\n\n- [x] done\n- [ ] todo");

        Assert.Contains("font-medium", md.Find("th").ClassName);
        Assert.Equal("2", md.FindAll("td")[1].TextContent);
        Assert.Equal(2, md.FindAll("input[type=checkbox]").Count);
    }

    [Fact]
    public void ReRendersAsTheContentStreamsIn()
    {
        using var ctx = new BunitContext();
        var md = Render(ctx, "Hel");

        md.Render(p => p.Add(c => c.Content, "Hello **world**"));

        Assert.Equal("world", md.Find("strong").TextContent);
    }

    [Theory]
    [InlineData("https://shellui.dev", "https://shellui.dev")]
    [InlineData("/docs?x=1#a", "/docs?x=1#a")]
    [InlineData("#top", "#top")]
    [InlineData("docs/page", "docs/page")]
    [InlineData("tel:+263", "tel:+263")]
    [InlineData("javascript:alert(1)", "")]
    [InlineData(" vbscript:x", "")]
    [InlineData("data:image/png;base64,AA", "")]
    public void SafeUrl_AllowsRelativeAndKnownSchemesOnly(string url, string expected)
    {
        Assert.Equal(expected, Markdown.SafeUrl(url));
    }

    [Fact]
    public void CodeBlock_ShowsTheFilenameAndCanHideTheCopyButton()
    {
        using var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var block = ctx.Render<CodeBlock>(p => p
            .Add(c => c.Code, "var x = 1;")
            .Add(c => c.Language, "csharp")
            .Add(c => c.Filename, "Program.cs")
            .Add(c => c.ShowCopy, false)
            .Add(c => c.Wrap, true));

        Assert.Contains("Program.cs", block.Markup);
        Assert.Empty(block.FindComponents<CopyButton>());
        Assert.Contains("whitespace-pre-wrap", block.Find("pre").ClassName);
        Assert.Equal("csharp", block.Find("code").GetAttribute("data-language"));
    }
}
