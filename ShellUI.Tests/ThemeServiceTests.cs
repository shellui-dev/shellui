using System.IO;
using System.Linq;
using ShellUI.CLI.Services;
using Xunit;

namespace ShellUI.Tests;

public class ThemeService_UrlNormalization
{
    [Theory]
    [InlineData("cmgy5f2vg000504l42pep9ob1", "https://tweakcn.com/r/themes/cmgy5f2vg000504l42pep9ob1")]
    [InlineData("https://tweakcn.com/themes/cmgy5f2vg000504l42pep9ob1", "https://tweakcn.com/r/themes/cmgy5f2vg000504l42pep9ob1")]
    [InlineData("https://tweakcn.com/themes/cmgy5f2vg000504l42pep9ob1?tab=colors", "https://tweakcn.com/r/themes/cmgy5f2vg000504l42pep9ob1")]
    [InlineData("https://tweakcn.com/r/themes/cmgy5f2vg000504l42pep9ob1", "https://tweakcn.com/r/themes/cmgy5f2vg000504l42pep9ob1")]
    [InlineData("http://tweakcn.com/themes/cmgy5f2vg000504l42pep9ob1", "https://tweakcn.com/r/themes/cmgy5f2vg000504l42pep9ob1")]
    public void NormalizeTweakcnUrl_HandlesAcceptedForms(string input, string expected)
    {
        Assert.Equal(expected, ThemeService.NormalizeTweakcnUrl(input));
    }

    [Theory]
    [InlineData("https://example.com/themes/abc")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("https://tweakcn.com/dashboard")]
    public void NormalizeTweakcnUrl_RejectsUnrecognizedInputs(string input)
    {
        Assert.Throws<System.ArgumentException>(() => ThemeService.NormalizeTweakcnUrl(input));
    }
}

public class ThemeService_JsonParsing
{
    private const string SampleTweakcnPayload = """
    {
      "$schema": "https://ui.shadcn.com/schema/registry-item.json",
      "name": "test-theme",
      "type": "registry:theme",
      "cssVars": {
        "theme": {
          "font-sans": "Inter, sans-serif",
          "radius": "0.75rem"
        },
        "light": {
          "background": "oklch(0.99 0 0)",
          "foreground": "oklch(0.10 0 0)",
          "primary": "oklch(0.55 0.15 250)"
        },
        "dark": {
          "background": "oklch(0.10 0 0)",
          "foreground": "oklch(0.95 0 0)"
        }
      }
    }
    """;

    [Fact]
    public void ParseTheme_ExtractsNameAndAllThreeVarDictionaries()
    {
        var theme = ThemeService.ParseTheme(SampleTweakcnPayload);

        Assert.Equal("test-theme", theme.Name);
        Assert.Equal(2, theme.ThemeVars.Count);
        Assert.Equal(3, theme.LightVars.Count);
        Assert.Equal(2, theme.DarkVars.Count);
        Assert.Equal("oklch(0.55 0.15 250)", theme.LightVars["primary"]);
        Assert.Equal("Inter, sans-serif", theme.ThemeVars["font-sans"]);
    }

    [Fact]
    public void ParseTheme_HandlesMissingSections()
    {
        var json = """{ "name": "minimal", "cssVars": { "light": { "background": "#fff" } } }""";
        var theme = ThemeService.ParseTheme(json);

        Assert.Equal("minimal", theme.Name);
        Assert.Empty(theme.ThemeVars);
        Assert.Empty(theme.DarkVars);
        Assert.Single(theme.LightVars);
    }
}

public class ThemeService_CssGeneration
{
    private static readonly TweakcnTheme Sample = new()
    {
        Name = "test",
        ThemeVars = new() { ["font-sans"] = "Inter, sans-serif", ["radius"] = "0.75rem" },
        LightVars = new() { ["background"] = "oklch(0.99 0 0)", ["foreground"] = "oklch(0.10 0 0)" },
        DarkVars = new() { ["background"] = "oklch(0.10 0 0)", ["foreground"] = "oklch(0.95 0 0)" },
    };

    [Fact]
    public void BuildThemeCss_EmitsSentinelMarkersAndBothScopes()
    {
        var css = ThemeService.BuildThemeCss(Sample);

        Assert.Contains(ThemeService.BeginMarker, css);
        Assert.Contains(ThemeService.EndMarker, css);
        Assert.Contains(":root {", css);
        Assert.Contains(".dark {", css);
        Assert.Contains("@theme inline {", css);
        Assert.Contains("--background: oklch(0.99 0 0);", css);
        Assert.Contains("--font-sans: Inter, sans-serif;", css);
    }

    [Fact]
    public void BuildThemeCss_OrdersVariablesDeterministically()
    {
        var css1 = ThemeService.BuildThemeCss(Sample);
        var css2 = ThemeService.BuildThemeCss(Sample);
        Assert.Equal(css1, css2);
    }

    [Fact]
    public void BuildThemeCss_SkipsSectionsWithNoVars()
    {
        var themeOnlyLight = new TweakcnTheme
        {
            Name = "x",
            ThemeVars = new(),
            LightVars = new() { ["background"] = "#fff" },
            DarkVars = new(),
        };
        var css = ThemeService.BuildThemeCss(themeOnlyLight);

        Assert.Contains(":root {", css);
        Assert.DoesNotContain(".dark {", css);
        Assert.DoesNotContain("@theme inline {", css);
    }
}

public class ThemeService_ApplyToInputCss
{
    private static TweakcnTheme MakeTheme(string tag = "themed") => new()
    {
        Name = tag,
        ThemeVars = new(),
        LightVars = new() { ["background"] = $"var(--{tag}-bg)" },
        DarkVars = new(),
    };

    [Fact]
    public void Apply_CreatesInputCssWithStarterContent_WhenFileMissing()
    {
        var dir = TempDir();
        var path = Path.Combine(dir, "input.css");
        try
        {
            ThemeService.ApplyToInputCss(path, MakeTheme());
            var content = File.ReadAllText(path);
            Assert.Contains("@import \"tailwindcss\";", content);
            Assert.Contains("@custom-variant dark", content);
            Assert.Contains(ThemeService.BeginMarker, content);
            Assert.Contains("var(--themed-bg)", content);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void Apply_PreservesUserContentAboveAndBelow_WhenSentinelExists()
    {
        var dir = TempDir();
        var path = Path.Combine(dir, "input.css");
        try
        {
            var original = "@import \"tailwindcss\";\n"
                + "@source \"./shellui-classes.txt\";\n"
                + "\n"
                + "/* user's custom utilities */\n"
                + ".my-custom { color: red; }\n"
                + "\n"
                + ThemeService.BeginMarker + "\n"
                + ":root { --background: oklch(0.99 0 0); }\n"
                + ThemeService.EndMarker + "\n"
                + "\n"
                + "/* more user content below */\n"
                + ".another-custom { color: blue; }\n";
            File.WriteAllText(path, original);

            ThemeService.ApplyToInputCss(path, MakeTheme("swapped"));
            var updated = File.ReadAllText(path);

            Assert.Contains("@source \"./shellui-classes.txt\";", updated);
            Assert.Contains(".my-custom { color: red; }", updated);
            Assert.Contains(".another-custom { color: blue; }", updated);
            Assert.Contains("var(--swapped-bg)", updated);
            Assert.DoesNotContain("oklch(0.99 0 0)", updated);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void Apply_AppendsBlock_WhenExistingFileHasNoSentinel()
    {
        var dir = TempDir();
        var path = Path.Combine(dir, "input.css");
        try
        {
            var original = "@import \"tailwindcss\";\n:root { --background: #fff; }\n";
            File.WriteAllText(path, original);

            ThemeService.ApplyToInputCss(path, MakeTheme());
            var updated = File.ReadAllText(path);

            Assert.Contains("@import \"tailwindcss\";", updated);
            Assert.Contains("--background: #fff", updated);
            Assert.Contains(ThemeService.BeginMarker, updated);
            Assert.Contains("var(--themed-bg)", updated);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void Apply_IsIdempotent_WithinSentinelRegion()
    {
        var dir = TempDir();
        var path = Path.Combine(dir, "input.css");
        try
        {
            ThemeService.ApplyToInputCss(path, MakeTheme());
            var first = File.ReadAllText(path);
            ThemeService.ApplyToInputCss(path, MakeTheme());
            var second = File.ReadAllText(path);

            Assert.Equal(first, second);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    private static string TempDir()
    {
        var d = Path.Combine(Path.GetTempPath(), "shellui-theme-test-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(d);
        return d;
    }
}

public class ThemeService_LockFile
{
    [Fact]
    public void WriteAndRead_RoundTrip()
    {
        var dir = Path.Combine(Path.GetTempPath(), "shellui-lock-test-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            ThemeService.WriteLockFile(dir, "https://tweakcn.com/r/themes/abc", "{\"name\":\"x\"}", "x");
            var lockFile = ThemeService.ReadLockFile(dir);

            Assert.NotNull(lockFile);
            Assert.Equal("https://tweakcn.com/r/themes/abc", lockFile!.SourceUrl);
            Assert.Equal("x", lockFile.ThemeName);
            Assert.Equal(64, lockFile.ContentSha256.Length); // SHA-256 hex is 64 chars
            Assert.Matches("^[a-f0-9]+$", lockFile.ContentSha256);
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void ReadLockFile_ReturnsNull_WhenMissing()
    {
        var dir = Path.Combine(Path.GetTempPath(), "shellui-lock-empty-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        try
        {
            Assert.Null(ThemeService.ReadLockFile(dir));
        }
        finally
        {
            Directory.Delete(dir, true);
        }
    }
}
