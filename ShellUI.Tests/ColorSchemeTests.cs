using System.IO;
using ShellUI.CLI.Services;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class ColorSchemeTests
{
    [Fact]
    public void InitTheme_SetsColorSchemeForBothThemes()
    {
        Assert.Contains("color-scheme: light;", CssTemplates.InputCss);
        Assert.Contains("color-scheme: dark;", CssTemplates.InputCss);
    }

    [Fact]
    public void AppliedTheme_SetsColorSchemeForBothThemes()
    {
        var css = ThemeService.BuildThemeCss(new TweakcnTheme
        {
            Name = "t",
            ThemeVars = new(),
            LightVars = new() { ["background"] = "#fff" },
            DarkVars = new() { ["background"] = "#000" },
        });

        Assert.Contains(":root {\n  color-scheme: light;", css.Replace("\r\n", "\n"));
        Assert.Contains(".dark {\n  color-scheme: dark;", css.Replace("\r\n", "\n"));
    }

    [Fact]
    public void PackageTheme_SetsColorSchemeForBothThemes()
    {
        var root = new DirectoryInfo(System.AppContext.BaseDirectory);
        while (root != null && !File.Exists(Path.Combine(root.FullName, "ShellUI.slnx"))) root = root.Parent;
        var css = File.ReadAllText(Path.Combine(root!.FullName, "src", "ShellUI.Components", "wwwroot", "shellui-theme.css"));

        Assert.Contains("color-scheme: light;", css);
        Assert.Contains("color-scheme: dark;", css);
    }
}
