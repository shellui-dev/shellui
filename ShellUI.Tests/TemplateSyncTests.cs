using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

/// Asserts the @code block of each live src/ShellUI.Components/Components/*.razor
/// matches the corresponding CLI template's emitted Content. Compares after stripping
/// comments, blank lines, and whitespace differences so the legitimate divergence
/// (namespace, formatting) doesn't fire, but real divergence (parameter list, JS
/// interop calls, lifecycle methods) does.
public class TemplateSyncTests
{
    // Component name → reason. Empty by default — fix the drift instead of adding entries.
    private static readonly Dictionary<string, string> AllowedDrift = new()
    {
    };

    [Theory]
    [InlineData("sidebar-trigger", "SidebarTrigger.razor")]
    [InlineData("theme-toggle", "ThemeToggle.razor")]
    [InlineData("input-otp", "InputOTP.razor")]
    public void TemplateCodeBlock_MatchesLiveLibrary(string templateName, string razorFileName)
    {
        if (AllowedDrift.ContainsKey(templateName)) return;

        var liveContent = File.ReadAllText(GetLiveRazorPath(razorFileName));
        var templateContent = ComponentRegistry.GetComponentContent(templateName)
            ?? throw new InvalidOperationException($"Template '{templateName}' not found in registry");

        var liveCode = ExtractCodeBlock(liveContent)
            ?? throw new InvalidOperationException($"Live {razorFileName} has no @code block");
        var templateCode = ExtractCodeBlock(templateContent)
            ?? throw new InvalidOperationException($"Template {templateName} has no @code block");

        var normalizedLive = Normalize(liveCode);
        var normalizedTemplate = Normalize(templateCode);

        Assert.True(normalizedLive == normalizedTemplate,
            $"Drift detected between live {razorFileName} and template {templateName}.\n" +
            $"This usually means someone updated one but not the other. Sync them, or add " +
            $"\"{templateName}\" to AllowedDrift in TemplateSyncTests with a reason.\n\n" +
            DiffSummary(normalizedLive, normalizedTemplate));
    }

    // [CallerFilePath] captures the absolute path of this source file at compile time,
    // so the test resolves the live components directory regardless of cwd on CI.
    private static string GetLiveRazorPath(string razorFileName, [CallerFilePath] string thisFile = "")
    {
        var testDir = Path.GetDirectoryName(thisFile) ?? throw new InvalidOperationException("CallerFilePath is empty");
        var repoRoot = Path.GetFullPath(Path.Combine(testDir, ".."));
        return Path.Combine(repoRoot, "src", "ShellUI.Components", "Components", razorFileName);
    }

    private static string Normalize(string code)
    {
        var withoutBlock = Regex.Replace(code, @"/\*.*?\*/", string.Empty, RegexOptions.Singleline);
        var lines = withoutBlock.Split('\n')
            .Select(l => Regex.Replace(l, @"//.*$", string.Empty))
            .Select(l => Regex.Replace(l.Trim(), @"\s+", " "))
            .Where(l => !string.IsNullOrWhiteSpace(l));
        return string.Join("\n", lines);
    }

    private static string DiffSummary(string live, string template)
    {
        var liveLines = live.Split('\n');
        var tmplLines = template.Split('\n');
        var max = Math.Max(liveLines.Length, tmplLines.Length);
        var diffs = new System.Text.StringBuilder();
        var shown = 0;
        for (var i = 0; i < max && shown < 5; i++)
        {
            var l = i < liveLines.Length ? liveLines[i] : "<missing>";
            var t = i < tmplLines.Length ? tmplLines[i] : "<missing>";
            if (l != t)
            {
                diffs.AppendLine($"  line {i + 1}");
                diffs.AppendLine($"    live:     {l}");
                diffs.AppendLine($"    template: {t}");
                shown++;
            }
        }
        return diffs.Length == 0 ? "(no per-line diff — file lengths differ)" : diffs.ToString();
    }

    /// Extracts the body of the first `@code { ... }` block, balancing braces while
    /// respecting strings, verbatim strings, char literals, line comments, and block comments.
    /// Returns null if no `@code` block is found or braces are unbalanced.
    private static string? ExtractCodeBlock(string razor)
    {
        var match = Regex.Match(razor, @"@code\s*\{");
        if (!match.Success) return null;

        var start = match.Index + match.Length;
        var depth = 1;
        var inString = false;
        var inVerbatimString = false;
        var inCharLiteral = false;
        var inLineComment = false;
        var inBlockComment = false;

        for (var i = start; i < razor.Length; i++)
        {
            var c = razor[i];
            var next = i + 1 < razor.Length ? razor[i + 1] : '\0';

            if (inLineComment)
            {
                if (c == '\n') inLineComment = false;
                continue;
            }
            if (inBlockComment)
            {
                if (c == '*' && next == '/') { inBlockComment = false; i++; }
                continue;
            }
            if (inVerbatimString)
            {
                if (c == '"' && next == '"') { i++; continue; }
                if (c == '"') inVerbatimString = false;
                continue;
            }
            if (inString)
            {
                if (c == '\\' && next != '\0') { i++; continue; }
                if (c == '"') inString = false;
                continue;
            }
            if (inCharLiteral)
            {
                if (c == '\\' && next != '\0') { i++; continue; }
                if (c == '\'') inCharLiteral = false;
                continue;
            }

            if (c == '/' && next == '/') { inLineComment = true; i++; continue; }
            if (c == '/' && next == '*') { inBlockComment = true; i++; continue; }
            if (c == '@' && next == '"') { inVerbatimString = true; i++; continue; }
            if (c == '"') { inString = true; continue; }
            if (c == '\'') { inCharLiteral = true; continue; }

            if (c == '{') depth++;
            else if (c == '}')
            {
                depth--;
                if (depth == 0) return razor.Substring(start, i - start);
            }
        }
        return null;
    }
}
