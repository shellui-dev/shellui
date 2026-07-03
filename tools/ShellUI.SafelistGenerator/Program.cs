using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ShellUI.SafelistGenerator;

/* Scans .razor and .cs files for Tailwind utility classes and emits:
     - wwwroot/shellui-classes.txt (committed, used for drift detection + demo build)
     - build/ShellUI.Components.targets (NuGet auto-imports; embeds the list inline so we don't
       depend on NuGet extracting a separate data file — behavior varies by client) */
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

    // Shared with tests so the drift check scans the same file set as the CLI.
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

    // .cs literals need filtering — without it, JS/CSS/error-message strings pollute the safelist.
    private static void ExtractFromCSharp(string content, SortedSet<string> sink)
    {
        foreach (Match lit in StringLiteralRegex.Matches(content))
        {
            var value = lit.Groups["lit"].Value;
            if (!LooksLikeClassList(value)) continue;
            HarvestTokens(value, sink);
        }
    }

    private static bool LooksLikeClassList(string s)
    {
        if (s.Length == 0 || s.Length > 500) return false;
        // `=` outside `[…]` implies key=value pairs, not class strings.
        if (s.Contains("://") || s.Contains('<') || s.Contains('>') || s.Contains('{')
            || s.Contains(';') || s.Contains('=') && !s.Contains('[')
            || s.Contains('\n')) return false;
        var tokens = s.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0) return false;
        var anyUtility = false;
        foreach (var tok in tokens)
        {
            if (tok.Length < 2) return false;
            var t = tok.TrimEnd(',', ';', ')', '"');
            if (t.Length < 2) return false;
            if (!char.IsLower(t[0]) && t[0] != '[' && t[0] != '!') return false;
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

    // XML comments in the generated .targets must not contain `--` (XML 1.0 spec); MSBuild on
    // Linux .NET 10.301+ rejects the file with MSB4024. Keep the emitted comment text safe.
    public static string BuildTargetsFileContent(IEnumerable<string> classes)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<Project>");
        sb.AppendLine("  <!-- AUTO-GENERATED by tools/ShellUI.SafelistGenerator. Do not edit by hand. -->");
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
        return s.Replace("&", "&amp;")
                .Replace("\"", "&quot;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
    }

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
