using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml;
using ShellUI.SafelistGenerator;
using Xunit;

namespace ShellUI.Tests;

/// Fails when a component uses Tailwind classes missing from the committed safelist.
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

        // Same file set as the CLI, or the diff reports false drift.
        var (razorFiles, csFiles) = Program.EnumerateSources(componentsDir);
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
        // XML 1.0 forbids `--` in comments, and strict MSBuild versions reject the file (MSB4024).
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
        // The packaged .targets must embed the same list as the .txt.
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
        msg += "Regenerate with:\n  dotnet run --project tools/ShellUI.SafelistGenerator -- src/ShellUI.Components/Components src/ShellUI.Components/wwwroot/shellui-classes.txt src/ShellUI.Components/build/ShellUI.Components.targets\n\n";
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
