using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using ShellUI.SafelistGenerator;
using Xunit;

namespace ShellUI.Tests;

/// Regenerates the safelist in-process and diffs against the committed file.
/// If a contributor adds a new Tailwind class to a component but forgets to
/// regenerate the safelist, this test fails with a precise diff of which
/// classes are missing — so NuGet consumers don't ship with broken styles.
public class SafelistDriftTests
{
    private const string RegenerateCommand =
        "dotnet run --project tools/ShellUI.SafelistGenerator -- "
        + "src/ShellUI.Components/Components "
        + "src/ShellUI.Components/wwwroot/shellui-classes.txt "
        + "src/ShellUI.Components/build/ShellUI.Components.targets";

    [Fact]
    public void Safelist_MatchesGeneratedFromCurrentSources()
    {
        var componentsDir = ResolveComponentsDir();
        var safelistPath = ResolveSafelistPath();

        Assert.True(Directory.Exists(componentsDir), $"components dir not found: {componentsDir}");
        Assert.True(File.Exists(safelistPath), $"safelist not found at {safelistPath}. Run: {RegenerateCommand}");

        // Match the generator's dual scan: .razor for components, .cs from the
        // parent Components/ root for variants/services that compose class strings
        // in C# (BadgeVariants, AlertVariants, etc.). Missing the .cs scan is why
        // Path A/B users hit incomplete Badge padding in 0.4.0-alpha.1.
        var razorFiles = Directory.GetFiles(componentsDir, "*.razor", SearchOption.AllDirectories);
        var componentsRoot = Path.GetDirectoryName(componentsDir.TrimEnd(Path.DirectorySeparatorChar))!;
        var csFiles = Directory.GetFiles(componentsRoot, "*.cs", SearchOption.AllDirectories);

        var freshlyGenerated = Program.GenerateSafelist(razorFiles.Concat(csFiles));
        var committed = File.ReadAllLines(safelistPath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToHashSet();

        var addedSinceCommit = freshlyGenerated.Except(committed).Take(10).ToList();
        var removedSinceCommit = committed.Except(freshlyGenerated).Take(10).ToList();

        Assert.True(addedSinceCommit.Count == 0 && removedSinceCommit.Count == 0,
            BuildDiffMessage(addedSinceCommit, removedSinceCommit));
    }

    [Fact]
    public void GeneratedTargetsFile_IsValidXml()
    {
        // XML 1.0 disallows `--` inside comment bodies. Some MSBuild versions are
        // permissive (Windows .NET 10.300 accepts it); strict parsers reject the
        // whole file with MSB4024. Run an actual XML parse so we catch this and
        // any other malformed-XML class of bug before the consumer's build does.
        var targetsPath = ResolveTargetsPath();
        Assert.True(File.Exists(targetsPath), $"targets file not found at {targetsPath}");

        var doc = new XmlDocument();
        var ex = Record.Exception(() => doc.Load(targetsPath));
        Assert.True(ex is null,
            $"build/ShellUI.Components.targets is not valid XML — strict MSBuild parsers will reject it.\n" +
            $"Likely cause: a `--` sequence inside an XML comment body. See the SafelistGenerator's comment template.\n\n" +
            $"Underlying error: {ex?.Message}");
    }

    [Fact]
    public void GeneratedTargetsFile_EmbedsSameClassesAsSafelist()
    {
        // The build/ShellUI.Components.targets file ships in the NuGet package and
        // writes the safelist into the consumer's wwwroot/ at build time. If the
        // embedded ItemGroup drifts from the .txt source-of-truth, NuGet consumers
        // see different classes than CLI consumers — bug class we want to never
        // ship.
        var safelistPath = ResolveSafelistPath();
        var targetsPath = ResolveTargetsPath();

        Assert.True(File.Exists(targetsPath), $"targets file not found at {targetsPath}. Run: {RegenerateCommand}");

        var committedClasses = File.ReadAllLines(safelistPath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToHashSet();
        var freshTargetsContent = Program.BuildTargetsFileContent(new SortedSet<string>(committedClasses, StringComparer.Ordinal));
        var committedTargetsContent = File.ReadAllText(targetsPath);

        Assert.True(freshTargetsContent == committedTargetsContent,
            $"build/ShellUI.Components.targets is out of sync with wwwroot/shellui-classes.txt. " +
            $"Regenerate with:\n  {RegenerateCommand}");
    }

    private static string BuildDiffMessage(System.Collections.Generic.List<string> added, System.Collections.Generic.List<string> removed)
    {
        var msg = "Safelist is out of date.\n";
        msg += "Regenerate with:\n  dotnet run --project tools/ShellUI.SafelistGenerator -- src/ShellUI.Components/Components src/ShellUI.Components/wwwroot/shellui-classes.txt\n\n";
        if (added.Count > 0)
        {
            msg += $"New classes in razor sources missing from safelist (first {added.Count}):\n";
            foreach (var c in added) msg += $"  + {c}\n";
        }
        if (removed.Count > 0)
        {
            msg += $"\nClasses in safelist but no longer used in razor sources (first {removed.Count}):\n";
            foreach (var c in removed) msg += $"  - {c}\n";
        }
        return msg;
    }

    private static string GetThisFilePath([CallerFilePath] string path = "") => path;

    private static string ResolveComponentsDir()
    {
        var testDir = Path.GetDirectoryName(GetThisFilePath())!;
        return Path.GetFullPath(Path.Combine(testDir, "..", "src", "ShellUI.Components", "Components"));
    }

    private static string ResolveSafelistPath()
    {
        var testDir = Path.GetDirectoryName(GetThisFilePath())!;
        return Path.GetFullPath(Path.Combine(testDir, "..", "src", "ShellUI.Components", "wwwroot", "shellui-classes.txt"));
    }

    private static string ResolveTargetsPath()
    {
        var testDir = Path.GetDirectoryName(GetThisFilePath())!;
        return Path.GetFullPath(Path.Combine(testDir, "..", "src", "ShellUI.Components", "build", "ShellUI.Components.targets"));
    }
}
