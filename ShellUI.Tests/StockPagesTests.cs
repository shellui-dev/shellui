using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ShellUI.CLI.Services;
using Xunit;

namespace ShellUI.Tests;

public class StockPagesTests
{
    [Theory]
    [InlineData("net10", "Home.razor")]
    [InlineData("net10", "Counter.razor")]
    [InlineData("net10", "Weather.razor")]
    [InlineData("net10", "Error.razor")]
    [InlineData("net8", "Weather.razor")]
    public void Restyle_ReplacesBootstrapInStockPages(string framework, string file)
    {
        var content = Fixture(framework, file);
        Assert.True(StockPages.IsStock(file, content));

        var restyled = StockPages.Restyle(file, content);

        Assert.False(StockPages.UsesBootstrap(restyled));
        Assert.DoesNotContain("class=\"table\"", restyled);
        Assert.DoesNotContain("<h1>", restyled);
        Assert.False(StockPages.IsStock(file, restyled));
        if (file != "Home.razor")
            Assert.Equal(restyled, StockPages.Restyle(file, restyled));
    }

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Restyle_TurnsStockHomeIntoThemeCard_KeepingLineEndings(string newline)
    {
        var stock = Fixture("net10", "Home.razor").Replace("\r\n", "\n").Replace("\n", newline);

        var home = StockPages.Restyle("Home.razor", stock);

        Assert.StartsWith("@page \"/\"", home);
        Assert.Contains("<PageTitle>Home</PageTitle>", home);
        Assert.Contains("shellui theme apply https://tweakcn.com/themes/&lt;id&gt;", home);
        Assert.Contains("href=\"https://tweakcn.com\"", home);
        Assert.Equal(newline == "\r\n", home.Contains("\r\n"));
    }

    [Fact]
    public void Restyle_KeepsCounterBehavior()
    {
        var restyled = StockPages.Restyle("Counter.razor", Fixture("net10", "Counter.razor"));

        Assert.Contains("@rendermode InteractiveServer", restyled);
        Assert.Contains("@onclick=\"IncrementCount\"", restyled);
        Assert.Contains("bg-primary", restyled);
    }

    [Fact]
    public void Apply_RestylesStockPagesAndReportsEditedOnes()
    {
        var root = Path.Combine(Path.GetTempPath(), "shellui-pages-" + Guid.NewGuid().ToString("N"));
        try
        {
            Write(root, "Components/Pages/Counter.razor", Fixture("net10", "Counter.razor"));
            Write(root, "Components/Pages/Weather.razor", Fixture("net10", "Weather.razor") + "\n<p>mine</p>");
            Write(root, "Components/Account/Pages/Login.razor", "<button class=\"w-100 btn btn-lg btn-primary\">Log in</button>");
            Write(root, "Components/UI/Button.razor", "<button class=\"btn\"></button>");

            var result = StockPages.Apply(root, new[] { "Components/UI", "Components/Layout" });

            Assert.Equal(new[] { "Components/Pages/Counter.razor" }, result.Restyled);
            Assert.False(StockPages.UsesBootstrap(File.ReadAllText(Path.Combine(root, "Components/Pages/Counter.razor"))));
            Assert.Single(result.Notes);
            Assert.Contains(result.Notes, n => n.Contains("Components/Pages/Weather.razor") && n.Contains("modified"));
            Assert.Contains("<p>mine</p>", File.ReadAllText(Path.Combine(root, "Components/Pages/Weather.razor")));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    private static void Write(string root, string relative, string content)
    {
        var path = Path.Combine(root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content);
    }

    private static string Fixture(string framework, string file, [CallerFilePath] string thisFile = "") =>
        File.ReadAllText(Path.Combine(Path.GetDirectoryName(thisFile)!, "Fixtures", "StockBlazor", framework, file + ".txt"));
}
