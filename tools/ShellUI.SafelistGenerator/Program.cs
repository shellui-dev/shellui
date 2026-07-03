using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ShellUI.SafelistGenerator;

/// Scans .razor files for Tailwind utility classes and emits two artifacts:
///
///   1. wwwroot/shellui-classes.txt — flat sorted list (committed for drift detection
///      and used by the live ShellUI demo's own Tailwind build).
///
///   2. build/ShellUI.Components.targets — NuGet-auto-imported MSBuild file that
///      embeds every class in an <ItemGroup> and writes them out to the consumer's
///      wwwroot/ during build. Self-contained — no second NuGet file to extract.
///
/// Usage:
///   dotnet run --project tools/ShellUI.SafelistGenerator -- <razorDir> <txtOut> <targetsOut>
public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.Error.WriteLine("usage: ShellUI.SafelistGenerator <razor-dir> <txt-out> <targets-out>");
            return 1;
        }

        var razorDir = args[0];
        var txtOut = args[1];
        var targetsOut = args[2];

        if (!Directory.Exists(razorDir))
        {
            Console.Error.WriteLine($"razor directory not found: {razorDir}");
            return 2;
        }

        var (razorFiles, csFiles) = EnumerateSources(razorDir);
        var classes = GenerateSafelist(razorFiles.Concat(csFiles));

        Directory.CreateDirectory(Path.GetDirectoryName(txtOut)!);
        File.WriteAllLines(txtOut, classes);

        Directory.CreateDirectory(Path.GetDirectoryName(targetsOut)!);
        File.WriteAllText(targetsOut, BuildTargetsFileContent(classes));

        Console.WriteLine($"wrote {classes.Count} classes from {razorFiles.Length} razor + {csFiles.Length} cs files");
        Console.WriteLine($"  txt:     {txtOut}");
        Console.WriteLine($"  targets: {targetsOut}");
        return 0;
    }

    /// Public — enumerates the source files the generator scans. Reused by tests
    /// so the drift check compares against the same file list the CLI would use.
    /// Both .razor components and .cs helpers get scanned (Variants, Services);
    /// bin/ and obj/ are excluded so contributors' build outputs don't taint output.
    public static (string[] razorFiles, string[] csFiles) EnumerateSources(string razorDir)
    {
        var razorFiles = Directory.GetFiles(razorDir, "*.razor", SearchOption.AllDirectories);
        var componentsRoot = Path.GetDirectoryName(razorDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
            ?? razorDir;
        var csFiles = Directory.GetFiles(componentsRoot, "*.cs", SearchOption.AllDirectories)
            .Where(p => !p.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
                     && !p.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            .ToArray();
        return (razorFiles, csFiles);
    }

    /// Public for in-process testing — runs the same extraction the CLI invocation does.
    public static SortedSet<string> GenerateSafelist(IEnumerable<string> sourceFilePaths)
    {
        var classes = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var file in sourceFilePaths)
        {
            var text = File.ReadAllText(file);
            if (file.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
            {
                ExtractClasses(text, classes);
            }
            else if (file.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
            {
                ExtractFromCSharp(text, classes);
            }
        }
        return classes;
    }

    // For .cs files (Variants, Services), pull tokens out of every string literal
    // — but only from literals that LOOK like a class-list string. Filters out
    // JS/HTML fragments, URLs, CSS property strings, error messages, and other
    // non-Tailwind literals that would otherwise pollute the safelist with tokens
    // like `background-color:`, `series[idx]`, `shadcn/ui.`, etc.
    private static void ExtractFromCSharp(string content, SortedSet<string> sink)
    {
        foreach (Match lit in StringLiteralRegex.Matches(content))
        {
            var value = lit.Groups["lit"].Value;
            if (!LooksLikeClassList(value)) continue;
            HarvestTokens(value, sink);
        }
    }

    // Heuristic: a class-list string is space-separated tokens, each looking like
    // a Tailwind utility. Reject literals that contain characters/patterns that
    // never appear in a real class list.
    private static bool LooksLikeClassList(string s)
    {
        if (s.Length == 0 || s.Length > 500) return false;
        // URLs, HTML/XML tags, JS fragments, C# format placeholders, CSS property syntax
        if (s.Contains("://") || s.Contains('<') || s.Contains('>') || s.Contains('{')
            || s.Contains(';') || s.Contains('=') && !s.Contains('[')  // `=` outside `[…]` = probably not a class
            || s.Contains('\n')) return false;
        // Must contain at least one token that looks Tailwindy: contains `-` or `:` or `[`
        // AND is composed of tokens that all satisfy the "utility class" shape.
        var tokens = s.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0) return false;
        var anyUtility = false;
        foreach (var tok in tokens)
        {
            if (tok.Length < 2) return false;
            var t = tok.TrimEnd(',', ';', ')', '"');
            if (t.Length < 2) return false;
            if (!char.IsLower(t[0]) && t[0] != '[' && t[0] != '!') return false;
            // Reject tokens with characters that never appear in a class name
            foreach (var c in t)
            {
                if (!(char.IsLetterOrDigit(c) || c is '-' or '_' or ':' or '/' or '.' or '[' or ']'
                    or '(' or ')' or '=' or '&' or ',' or '!' or '%' or '#' or '@' or '*'))
                    return false;
            }
            if (t.Contains('-') || t.Contains(':') || t.Contains('[') || t.Contains('/'))
                anyUtility = true;
        }
        return anyUtility;
    }

    /// Public for in-process testing — produces the same .targets content the CLI writes.
    public static string BuildTargetsFileContent(IEnumerable<string> classes)
    {
        // XML comments cannot contain a `--` sequence (XML 1.0 spec). Some MSBuild
        // versions accept it; others (Linux .NET 10.301+) reject the whole file
        // with MSB4024. Avoid `--` anywhere in the comment text — that's why we
        // don't show example CLI flags here. Regen instructions live in the tool's
        // own help output: `dotnet run --project tools/ShellUI.SafelistGenerator`.
        var sb = new StringBuilder();
        sb.AppendLine("<Project>");
        sb.AppendLine("  <!--");
        sb.AppendLine("    AUTO-GENERATED by tools/ShellUI.SafelistGenerator. Do not edit by hand.");
        sb.AppendLine();
        sb.AppendLine("    NuGet auto-imports build/<PackageId>.targets into any consumer build. We");
        sb.AppendLine("    embed the Tailwind safelist inline so we do not depend on NuGet extracting");
        sb.AppendLine("    a separate data file (behavior varies by client and version). At consumer");
        sb.AppendLine("    build time, WriteLinesToFile drops the list into wwwroot so input.css can");
        sb.AppendLine("    reference it via @source.");
        sb.AppendLine("  -->");
        sb.AppendLine("  <ItemGroup>");
        foreach (var cls in classes)
        {
            sb.Append("    <ShellUISafelistEntry Include=\"");
            sb.Append(XmlEscape(cls));
            sb.AppendLine("\" />");
        }
        sb.AppendLine("  </ItemGroup>");
        sb.AppendLine();
        sb.AppendLine("  <Target Name=\"WriteShellUISafelist\"");
        sb.AppendLine("          BeforeTargets=\"BeforeBuild;PrepareForBuild\"");
        sb.AppendLine("          Condition=\"'$(SkipShellUISafelist)' != 'true'\">");
        sb.AppendLine("    <MakeDir Directories=\"$(MSBuildProjectDirectory)/wwwroot\" Condition=\"!Exists('$(MSBuildProjectDirectory)/wwwroot')\" />");
        sb.AppendLine("    <WriteLinesToFile File=\"$(MSBuildProjectDirectory)/wwwroot/shellui-classes.txt\"");
        sb.AppendLine("                      Lines=\"@(ShellUISafelistEntry)\"");
        sb.AppendLine("                      Overwrite=\"true\" />");
        sb.AppendLine("  </Target>");
        sb.AppendLine("</Project>");
        return sb.ToString();
    }

    private static string XmlEscape(string s)
    {
        // We control the input (extracted Tailwind class strings) but `&` and `"` show
        // up in arbitrary values like data-[state=on] — escape conservatively.
        return s.Replace("&", "&amp;")
                .Replace("\"", "&quot;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
    }

    // Class-attribute extraction — handles literal strings, Razor expressions, nested
    // Shell.Cn("foo", …) literals. Razor variable parts (`@Class`) are dropped because
    // Tailwind would not see those at build time anyway.
    private static readonly Regex ClassAttributeRegex = new(
        "class\\s*=\\s*\"(?<value>[^\"]*)\"",
        RegexOptions.Compiled);

    private static readonly Regex StringLiteralRegex = new(
        "\"(?<lit>[^\"]*)\"",
        RegexOptions.Compiled);

    private static void ExtractClasses(string content, SortedSet<string> sink)
    {
        foreach (Match attr in ClassAttributeRegex.Matches(content))
        {
            var value = attr.Groups["value"].Value;
            HarvestTokens(value, sink);
            foreach (Match lit in StringLiteralRegex.Matches(value))
            {
                HarvestTokens(lit.Groups["lit"].Value, sink);
            }
        }
    }

    // A token qualifies as a Tailwind utility class if it contains at least one of
    // `-`, `:`, `[`, `/` AND starts with a lowercase letter. Excludes C# identifiers
    // (`Class`, `Variant`) caught by our literal extraction.
    private static void HarvestTokens(string text, SortedSet<string> sink)
    {
        var tokens = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var raw in tokens)
        {
            var token = raw.Trim();
            if (token.Length < 2) continue;
            if (!char.IsLower(token[0])) continue;
            if (!token.Contains('-') && !token.Contains(':') && !token.Contains('[') && !token.Contains('/')) continue;
            token = token.TrimEnd(',', ';', ')', '"');
            if (token.Length < 2) continue;
            sink.Add(token);
        }
    }
}
