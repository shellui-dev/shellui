using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class ClassParameterTests
{
    private static readonly Regex DeclaresClassName = new(@"public string\?? ClassName\b");
    private static readonly Regex DeclaresClass = new(@"public string\? Class \{");
    private static readonly Regex RendersClass = new(@"@Class\b|[,+]\s*Class\)");

    [Fact]
    public void EveryTemplateWithClassName_AlsoAcceptsAndRendersClass()
    {
        var offenders = new List<string>();
        foreach (var (name, _) in ComponentRegistry.Components)
        {
            var content = ComponentRegistry.GetComponentContent(name);
            if (content is null || !DeclaresClassName.IsMatch(content)) continue;

            if (!DeclaresClass.IsMatch(content)) offenders.Add($"{name}: no `Class` parameter");
            else if (!RendersClass.IsMatch(content)) offenders.Add($"{name}: `Class` declared but never rendered");
        }

        Assert.True(offenders.Count == 0,
            "Templates that expose ClassName must also accept and render Class:\n  " +
            string.Join("\n  ", offenders));
    }

    [Fact]
    public void EveryPackageComponentWithClassName_AlsoAcceptsAndRendersClass()
    {
        var offenders = new List<string>();
        foreach (var path in Directory.GetFiles(GetComponentsDirectory(), "*.razor"))
        {
            var content = File.ReadAllText(path);
            if (!DeclaresClassName.IsMatch(content)) continue;

            var file = Path.GetFileName(path);
            if (!DeclaresClass.IsMatch(content)) offenders.Add($"{file}: no `Class` parameter");
            else if (!RendersClass.IsMatch(content)) offenders.Add($"{file}: `Class` declared but never rendered");
        }

        Assert.True(offenders.Count == 0,
            "Package components that expose ClassName must also accept and render Class:\n  " +
            string.Join("\n  ", offenders));
    }

    private static string GetComponentsDirectory([CallerFilePath] string thisFile = "")
    {
        var testDir = Path.GetDirectoryName(thisFile) ?? throw new InvalidOperationException("CallerFilePath is empty");
        return Path.GetFullPath(Path.Combine(testDir, "..", "src", "ShellUI.Components", "Components"));
    }
}
