using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

/// Verifies that the *generated* content of each template parses as valid C#.
/// Pure-.cs templates (variants) parse the whole content; .razor templates parse
/// the body of the @code block. Catches unescaped quotes inside C# verbatim
/// strings — the kind of error that ships as a compile failure to consumers.
public class TemplateCompileTests
{
    [Theory]
    [InlineData("chart-variants")]
    [InlineData("alert-variants")]
    [InlineData("badge-variants")]
    [InlineData("button-variants")]
    public void CsharpTemplate_GeneratedContentParses(string componentName)
    {
        var content = ComponentRegistry.GetComponentContent(componentName);
        Assert.False(string.IsNullOrWhiteSpace(content), $"{componentName} template has no content");

        var tree = CSharpSyntaxTree.ParseText(content!);
        var errors = tree.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToList();

        Assert.True(errors.Count == 0,
            $"{componentName} generated content has {errors.Count} parse error(s):\n" +
            string.Join("\n", errors.Select(e => $"  {e.Location.GetLineSpan().StartLinePosition}: {e.GetMessage()}")));
    }

    [Theory]
    [InlineData("pie-chart")]
    [InlineData("dashboard-02")]
    [InlineData("button")]
    [InlineData("dialog")]
    public void RazorTemplate_CodeBlockParses(string componentName)
    {
        var content = ComponentRegistry.GetComponentContent(componentName);
        Assert.False(string.IsNullOrWhiteSpace(content), $"{componentName} template has no content");

        var codeBlock = ExtractCodeBlock(content!);
        if (string.IsNullOrWhiteSpace(codeBlock))
        {
            // Brace extraction failed — almost always because an unterminated string
            // swallowed the closing brace. Surface a real diagnostic by parsing the
            // raw @code-onward suffix as if it were C#; the line/column points at
            // the actual offending escape.
            var raw = StripRazorDirectives(content!);
            var rawTree = CSharpSyntaxTree.ParseText($"class __Probe {{ {raw} }}");
            var rawErrors = rawTree.GetDiagnostics()
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                // The only diagnostics that matter for this bug class are unterminated literals.
                .Where(d => d.Id is "CS1010" or "CS1002" or "CS1003" or "CS1026" or "CS1513" or "CS1525" or "CS1056")
                .Take(5)
                .ToList();
            Assert.Fail(
                $"{componentName} @code block could not be extracted — likely an unterminated " +
                $"string literal in the template. Diagnostics:\n" +
                string.Join("\n", rawErrors.Select(e => $"  {e.Id} at {e.Location.GetLineSpan().StartLinePosition}: {e.GetMessage()}")));
        }

        // Wrap in a synthetic class so the code block parses standalone.
        var wrapped = $"class __Probe {{ {codeBlock} }}";
        var tree = CSharpSyntaxTree.ParseText(wrapped);
        var errors = tree.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToList();

        Assert.True(errors.Count == 0,
            $"{componentName} @code block has {errors.Count} parse error(s):\n" +
            string.Join("\n", errors.Select(e => $"  {e.Location.GetLineSpan().StartLinePosition}: {e.GetMessage()}")));
    }

    /// Strips Razor markup directives so the remaining text can be best-effort
    /// parsed as C#. Not a real Razor parser — just enough to surface useful
    /// diagnostics when ExtractCodeBlock fails.
    private static string StripRazorDirectives(string razor)
    {
        // Drop everything before the first @code keyword if present, else return as-is.
        var codeIdx = razor.IndexOf("@code", System.StringComparison.Ordinal);
        return codeIdx >= 0 ? razor.Substring(codeIdx + 5) : razor;
    }

    /// Extracts the body of the first `@code { ... }` block, balancing braces.
    /// Returns null if no `@code` block is found.
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
                if (c == '"' && next == '"') { i++; continue; } // escaped ""
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
