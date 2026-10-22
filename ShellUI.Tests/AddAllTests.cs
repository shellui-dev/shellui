using ShellUI.CLI.Services;
using ShellUI.Templates;

namespace ShellUI.Tests;

public class AddAllTests
{
    [Fact]
    public void LibraryComponents_IsEveryListedTargetExceptLayoutBlocks()
    {
        var all = ComponentInstaller.LibraryComponents().ToList();
        var listed = ComponentRegistry.Components.Values.Where(m => m.IsAvailable).ToList();

        Assert.Equal(listed.Count(m => !m.IsLayoutBlock), all.Count);
        Assert.Contains("button", all);
        Assert.Contains("chart", all);
        Assert.DoesNotContain("dashboard-02", all);
        Assert.DoesNotContain("auth-01", all);
        Assert.DoesNotContain("sidebar-account", all);
        Assert.DoesNotContain("tab-models", all);
    }

    [Theory]
    [InlineData("bar-chart", "chart", true)]
    [InlineData("multi-series-chart", "chart", true)]
    [InlineData("button", "chart", false)]
    [InlineData("dialog", "button", false)]
    public void DependsOnAny_FollowsDependenciesTransitively(string name, string excluded, bool expected)
    {
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { excluded };
        Assert.Equal(expected, ComponentInstaller.DependsOnAny(name, names));
    }
}
