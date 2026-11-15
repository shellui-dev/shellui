using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

/// Most CLI templates read the ShellUI.Components source through ComponentSource, so they can't drift from it.
/// These tests keep it that way, and compare the few hand-written templates with the package.
public class TemplateSyncTests
{
    // Templates that keep their own copy of a package file, and why. Any other template with a package file must read it.
    private static readonly Dictionary<string, string> HandWritten = new()
    {
        ["command-palette"] = "JS interop: the package imports the shellui.js module; the template calls the window.ShellUI global.",
        ["input-otp"] = "JS interop: the package imports the shellui.js module (with a fallback); the template calls the window.ShellUI global.",
        ["copy-button"] = "JS interop: the package imports the shellui.js module; the template calls the window.ShellUI global.",
        ["sidebar-provider"] = "JS interop: the package imports shellui-sidebar.js; the template calls window.ShellUI.initSidebar, since a dynamic import 404s once compiled into a consumer's class library.",
        ["app-sidebar"] = "The template is the dashboard block that init rewrites with the app's routes; the package copy is a sample.",
        ["sonner"] = "Installs with sonner-variants into YourProjectNamespace.Components, not Components.UI.",
        ["sonner-variants"] = "Installs into YourProjectNamespace.Components, not Components.UI."
    };

    // Hand-written templates whose @code differs on purpose (the JS interop above); their markup is still compared.
    private static readonly HashSet<string> CodeDiffers = new() { "command-palette", "input-otp", "copy-button", "sidebar-provider", "app-sidebar" };

    // app-sidebar's markup is the block init fills with the app's routes, not the package's sample.
    private static readonly HashSet<string> MarkupDiffers = new() { "app-sidebar" };

    // Registry entries with a ShellUI.Components file of the same name: template name, path inside the package.
    public static IEnumerable<object[]> PackageBacked() =>
        ComponentRegistry.Components.Values
            .Where(m => !m.IsLayoutBlock)
            .Select(m => (m.Name, Path: PackagePath(Path.GetFileName(m.FilePath))))
            .Where(t => t.Path != null)
            .Select(t => new object[] { t.Name, t.Path! });

    public static IEnumerable<object[]> HandWrittenRazor() =>
        PackageBacked().Where(row => HandWritten.ContainsKey((string)row[0]) && ((string)row[1]).EndsWith(".razor"));

    [Theory]
    [MemberData(nameof(PackageBacked))]
    public void Template_ReadsThePackageSource_UnlessHandWritten(string templateName, string packagePath)
    {
        var template = ComponentRegistry.GetComponentContent(templateName)!;
        var readsPackage = template == ComponentSource.Read(packagePath) || template == ComponentSource.Read(packagePath, variantsNamespace: true);

        if (HandWritten.ContainsKey(templateName))
            Assert.False(readsPackage, $"{templateName} now matches {packagePath}; read it with ComponentSource and remove it from HandWritten.");
        else
            Assert.True(readsPackage, $"{templateName} doesn't read {packagePath}. Use ComponentSource.Read, or add it to HandWritten with a reason.");
    }

    [Theory]
    [MemberData(nameof(HandWrittenRazor))]
    public void HandWrittenTemplate_CodeMatchesPackage(string templateName, string packagePath)
    {
        if (CodeDiffers.Contains(templateName)) return;

        var liveCode = ExtractCodeBlock(File.ReadAllText(PackageFile(packagePath)))
            ?? throw new InvalidOperationException($"{packagePath} has no @code block");
        var templateCode = ExtractCodeBlock(ComponentRegistry.GetComponentContent(templateName)!)
            ?? throw new InvalidOperationException($"Template {templateName} has no @code block");

        var live = Normalize(liveCode);
        var template = Normalize(templateCode);
        Assert.True(live == template,
            $"Drift between {packagePath} and the hand-written template {templateName}. Sync them.\n\n" + DiffSummary(live, template));
    }

    [Theory]
    [MemberData(nameof(HandWrittenRazor))]
    public void HandWrittenTemplate_MarkupMatchesPackage(string templateName, string packagePath)
    {
        if (MarkupDiffers.Contains(templateName)) return;

        var live = Normalize(ExtractMarkup(File.ReadAllText(PackageFile(packagePath))));
        var template = Normalize(ExtractMarkup(ComponentRegistry.GetComponentContent(templateName)!));

        Assert.True(live == template,
            $"Markup drift between {packagePath} and the hand-written template {templateName}.\n\n" + DiffSummary(live, template));
    }

    [Fact]
    public void HandWritten_NamesRegistryTemplates()
    {
        foreach (var name in HandWritten.Keys)
            Assert.True(ComponentRegistry.Components.ContainsKey(name), $"HandWritten lists {name}, which isn't in the registry.");
    }

    // The folders ComponentSource embeds; file names are unique across them.
    private static readonly string[] PackageFolders = { "Components", "Variants", "Models", "Services", "" };

    private static string? PackagePath(string fileName) =>
        PackageFolders.Select(folder => folder.Length == 0 ? fileName : $"{folder}/{fileName}")
            .FirstOrDefault(rel => File.Exists(PackageFile(rel)));

    private static string PackageFile(string relativePath, [CallerFilePath] string thisFile = "") =>
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(thisFile)!, "..", "src", "ShellUI.Components", relativePath));

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

    // Everything before `@code`, without the @namespace and @using lines that differ by design.
    private static string ExtractMarkup(string razor)
    {
        var match = Regex.Match(razor, @"@code\s*\{");
        var head = match.Success ? razor.Substring(0, match.Index) : razor;
        var lines = head.Split('\n').Where(l => !Regex.IsMatch(l.Trim(), @"^@(namespace|using)\b"));
        return string.Join("\n", lines);
    }

    /// Body of the first `@code { }` block, or null if missing or unbalanced.
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
