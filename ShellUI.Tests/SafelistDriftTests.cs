using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ShellUI.SafelistGenerator;
using Xunit;

namespace ShellUI.Tests;

/// Regenerates the safelist in-process and diffs against the committed file.
/// If a contributor adds a new Tailwind class to a component but forgets to
/// regenerate the safelist, this test fails with a precise diff of which
/// classes are missing — so NuGet consumers don't ship with broken styles.
public class SafelistDriftTests
{
    [Fact]
    public void Safelist_MatchesGeneratedFromCurrentRazorSources()
    {
        var componentsDir = ResolveComponentsDir();
        var safelistPath = ResolveSafelistPath();

        Assert.True(Directory.Exists(componentsDir), $"components dir not found: {componentsDir}");
        Assert.True(File.Exists(safelistPath),
            $"safelist not found at {safelistPath}. Run: dotnet run --project tools/ShellUI.SafelistGenerator -- src/ShellUI.Components/Components src/ShellUI.Components/wwwroot/shellui-classes.txt");

        var razorFiles = Directory.GetFiles(componentsDir, "*.razor", SearchOption.AllDirectories);
        var freshlyGenerated = Program.GenerateSafelist(razorFiles);
        var committed = File.ReadAllLines(safelistPath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToHashSet();

        var addedSinceCommit = freshlyGenerated.Except(committed).Take(10).ToList();
        var removedSinceCommit = committed.Except(freshlyGenerated).Take(10).ToList();

        Assert.True(addedSinceCommit.Count == 0 && removedSinceCommit.Count == 0,
            BuildDiffMessage(addedSinceCommit, removedSinceCommit));
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
}
