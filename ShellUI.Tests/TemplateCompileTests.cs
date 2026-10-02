using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ShellUI.Core.Models;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

/// Parses each template's generated C# to catch unescaped quotes in verbatim strings.
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
    [InlineData("tabs")]
    [InlineData("select")]
    [InlineData("sidebar-provider")]
    public void RazorTemplate_CodeBlockParses(string componentName)
    {
        var content = ComponentRegistry.GetComponentContent(componentName);
        Assert.False(string.IsNullOrWhiteSpace(content), $"{componentName} template has no content");

        var codeBlock = ExtractCodeBlock(content!);
        if (string.IsNullOrWhiteSpace(codeBlock))
        {
            // Usually an unterminated string swallowed the brace; parsing the rest points at it.
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

        var wrapped = $"class __Probe {{ {codeBlock} }}";
        var tree = CSharpSyntaxTree.ParseText(wrapped);
        var errors = tree.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToList();

        Assert.True(errors.Count == 0,
            $"{componentName} @code block has {errors.Count} parse error(s):\n" +
            string.Join("\n", errors.Select(e => $"  {e.Location.GetLineSpan().StartLinePosition}: {e.GetMessage()}")));
    }

    /// Lists every failing template instead of stopping at the first.
    [Fact]
    public void EveryRazorTemplate_CodeBlockParses()
    {
        var failures = new List<string>();
        foreach (var (name, metadata) in ComponentRegistry.Components)
        {
            if (!metadata.FilePath.EndsWith(".razor", System.StringComparison.OrdinalIgnoreCase)) continue;

            var content = ComponentRegistry.GetComponentContent(name);
            if (string.IsNullOrWhiteSpace(content)) continue;
            if (!content.Contains("@code")) continue;

            var codeBlock = ExtractCodeBlock(content!);
            if (string.IsNullOrWhiteSpace(codeBlock))
            {
                failures.Add($"{name}: @code block could not be extracted (likely unterminated string literal)");
                continue;
            }

            var wrapped = $"class __Probe {{ {codeBlock} }}";
            var errors = CSharpSyntaxTree.ParseText(wrapped).GetDiagnostics()
                .Where(d => d.Severity == DiagnosticSeverity.Error)
                .Select(d => $"{d.Location.GetLineSpan().StartLinePosition}: {d.GetMessage()}")
                .ToList();
            if (errors.Count > 0)
                failures.Add($"{name}: {errors.Count} parse error(s) — {errors[0]}");
        }
        Assert.True(failures.Count == 0,
            "The following templates have parse errors in their @code block:\n  " +
            string.Join("\n  ", failures));
    }

    [Fact]
    public void EveryRegistryEntry_HasContent()
    {
        var empty = ComponentRegistry.Components.Keys
            .Where(name => string.IsNullOrWhiteSpace(ComponentRegistry.GetComponentContent(name)))
            .ToList();
        Assert.True(empty.Count == 0, "Registry entries with empty content:\n  " + string.Join("\n  ", empty));
    }

    [Fact]
    public void EveryHiddenEntry_IsReachableFromAnInstallableTarget()
    {
        var reachable = new HashSet<string>();
        var stack = new Stack<string>(ComponentRegistry.Components.Where(c => c.Value.IsAvailable).Select(c => c.Key));
        while (stack.Count > 0)
        {
            var name = stack.Pop();
            if (!reachable.Add(name)) continue;
            foreach (var dep in ComponentRegistry.Components[name].Dependencies ?? new List<string>())
                stack.Push(dep);
        }
        // sidebar-js: legacy, kept for projects that still load shellui-sidebar.js.
        var orphans = ComponentRegistry.Components.Keys.Where(k => !reachable.Contains(k) && k != "sidebar-js").ToList();
        Assert.True(orphans.Count == 0, "Hidden entries no installable target depends on:\n  " + string.Join("\n  ", orphans));
    }

    // `shellui add <target>` alone must compile: every project namespace a file imports has to be declared
    // by a file the same install writes. The all-components CI sweep can't see this, since other files fill the gap.
    [Fact]
    public void EveryDirectTarget_ImportsOnlyNamespacesItInstalls()
    {
        var declaration = new Regex(@"^\s*@?namespace\s+YourProjectNamespace(\.[\w.]+)?", RegexOptions.Multiline);
        var import = new Regex(@"^\s*@?using\s+YourProjectNamespace(\.[\w.]+)?\s*;?\s*$", RegexOptions.Multiline);
        // Present in every project: init installs shell and shellui-js, and files without @namespace use folder namespaces.
        var always = new[] { "", ".Components", ".Components.UI", ".Components.Layout" };

        var failures = new List<string>();
        foreach (var (name, metadata) in ComponentRegistry.Components.Where(c => c.Value.IsAvailable))
        {
            var closure = new HashSet<string>();
            var stack = new Stack<string>(new[] { name, "shell", "shellui-js" });
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (!closure.Add(current)) continue;
                foreach (var dep in ComponentRegistry.Components[current].Dependencies ?? new List<string>())
                    stack.Push(dep);
            }

            var contents = closure.Select(n => ComponentRegistry.GetComponentContent(n) ?? "").ToList();
            var declared = contents.SelectMany(c => declaration.Matches(c).Select(m => m.Groups[1].Value)).Concat(always).ToHashSet();
            var missing = contents.SelectMany(c => import.Matches(c).Select(m => m.Groups[1].Value))
                .Where(ns => !declared.Contains(ns))
                .Distinct()
                .ToList();
            if (missing.Count > 0)
                failures.Add($"{name}: {string.Join(", ", missing.Select(ns => "YourProjectNamespace" + ns))}");

            var packages = closure.SelectMany(n => ComponentRegistry.Components[n].NuGetDependencies ?? new List<NuGetDependency>())
                .Select(p => p.PackageId)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var (ns, package) in ThirdPartyNamespaces)
            {
                var usesIt = new Regex($@"^\s*@?using\s+{Regex.Escape(ns)}\s*;?\s*$", RegexOptions.Multiline);
                if (contents.Any(c => usesIt.IsMatch(c)) && !packages.Contains(package))
                    failures.Add($"{name}: imports {ns} without the {package} package");
            }
        }

        Assert.True(failures.Count == 0, "Targets whose imports are not installed with them:\n  " + string.Join("\n  ", failures));
    }

    private static readonly (string Namespace, string Package)[] ThirdPartyNamespaces =
    {
        ("ApexCharts", "Blazor-ApexCharts"),
        ("System.Linq.Dynamic.Core", "System.Linq.Dynamic.Core")
    };

    /// Best effort: drops everything before @code so the rest can be parsed as C#.
    private static string StripRazorDirectives(string razor)
    {
        var codeIdx = razor.IndexOf("@code", System.StringComparison.Ordinal);
        return codeIdx >= 0 ? razor.Substring(codeIdx + 5) : razor;
    }

    /// Body of the first `@code { }` block, or null.
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
