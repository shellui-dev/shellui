using System.Text.RegularExpressions;

namespace ShellUI.SafelistGenerator;

/// Scans .razor files for Tailwind utility classes and emits a sorted, unique
/// safelist consumers can reference from their input.css via `@source "..."`.
///
/// Usage: dotnet run --project tools/ShellUI.SafelistGenerator -- <sourceDir> <outputFile>
public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.Error.WriteLine("usage: ShellUI.SafelistGenerator <source-dir> <output-file>");
            return 1;
        }

        var sourceDir = args[0];
        var outputFile = args[1];

        if (!Directory.Exists(sourceDir))
        {
            Console.Error.WriteLine($"source directory not found: {sourceDir}");
            return 2;
        }

        var razorFiles = Directory.GetFiles(sourceDir, "*.razor", SearchOption.AllDirectories);
        var classes = GenerateSafelist(razorFiles);

        Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);
        File.WriteAllLines(outputFile, classes);

        Console.WriteLine($"wrote {classes.Count} unique classes from {razorFiles.Length} razor files → {outputFile}");
        return 0;
    }

    /// Public for in-process testing — runs the same extraction the CLI invocation does.
    public static SortedSet<string> GenerateSafelist(IEnumerable<string> razorFilePaths)
    {
        var classes = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var file in razorFilePaths)
        {
            ExtractClasses(File.ReadAllText(file), classes);
        }
        return classes;
    }

    // Razor class attributes take many forms. We pull string literals out of every form;
    // Razor interpolations like `@(IsActive ? "foo" : "bar")` contribute their literal
    // branches but the variable parts are dropped (Tailwind wouldn't see them anyway).
    private static readonly Regex ClassAttributeRegex = new(
        "class\\s*=\\s*\"(?<value>[^\"]*)\"",
        RegexOptions.Compiled);

    // String literals nested inside the attribute value — e.g. Shell.Cn("foo bar", …).
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
    // the disambiguating characters (`-`, `:`, `[`, `/`) AND starts with a lowercase
    // letter. This excludes C# identifiers like `Class`, `Variant`, `IsActive` that
    // can show up inside Razor expressions caught by our literal extraction.
    private static void HarvestTokens(string text, SortedSet<string> sink)
    {
        var tokens = text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var raw in tokens)
        {
            var token = raw.Trim();
            if (token.Length < 2) continue;
            if (!char.IsLower(token[0])) continue;
            if (!token.Contains('-') && !token.Contains(':') && !token.Contains('[') && !token.Contains('/')) continue;
            // Strip leading punctuation residue (commas, quotes) from poorly-tokenized fragments.
            token = token.TrimEnd(',', ';', ')', '"');
            if (token.Length < 2) continue;
            sink.Add(token);
        }
    }
}
