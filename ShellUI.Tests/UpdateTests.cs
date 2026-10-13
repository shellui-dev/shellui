using System.Collections.Generic;
using System.IO;
using ShellUI.CLI.Services;
using ShellUI.Core.Models;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class UpdateDependencyTests
{
    [Fact]
    public void MissingDependencies_FindsDependenciesAddedSinceInstall()
    {
        // A 0.3 install of tabs and breadcrumb, before tab-models and the breadcrumb parts existed.
        var installed = new List<string> { "tabs", "tabs-list", "tabs-trigger", "tabs-content", "breadcrumb", "breadcrumb-item" };

        var missing = ComponentManager.MissingDependencies(new[] { "tabs", "breadcrumb" }, installed);

        Assert.Contains("tab-models", missing);
        Assert.Contains("breadcrumb-list", missing);
        Assert.Contains("breadcrumb-ellipsis", missing);
        Assert.DoesNotContain("tabs-list", missing);
        Assert.Equal(missing.Count, new HashSet<string>(missing).Count);
    }

    [Fact]
    public void MissingDependencies_IsEmptyWhenEverythingIsInstalled()
    {
        var tabs = ComponentRegistry.GetMetadata("tabs")!;
        var installed = new List<string>(tabs.Dependencies) { "tabs" };

        Assert.Empty(ComponentManager.MissingDependencies(new[] { "tabs" }, installed));
    }
}

public class UpdateLocalChangesTests
{
    private const string Template = "<div class=\"p-4\">@ChildContent</div>\n";

    private static InstalledComponent Entry(string name, string? hash) => new() { Name = name, Version = "0.4.0", Hash = hash };

    [Fact]
    public void FileMatchingTheRecordedHash_HasNoLocalChanges()
    {
        var entry = Entry("button", ComponentInstaller.ContentHash(Template));

        Assert.False(ComponentManager.HasLocalChanges(entry, ComponentRegistry.GetMetadata("button")!, Template, Template));
    }

    [Fact]
    public void EditedFile_HasLocalChanges()
    {
        var entry = Entry("button", ComponentInstaller.ContentHash(Template));

        Assert.True(ComponentManager.HasLocalChanges(entry, ComponentRegistry.GetMetadata("button")!, Template + "<!-- mine -->", Template));
    }

    [Fact]
    public void LineEndingChangesAreNotLocalChanges()
    {
        var entry = Entry("button", ComponentInstaller.ContentHash(Template));

        Assert.False(ComponentManager.HasLocalChanges(entry, ComponentRegistry.GetMetadata("button")!, Template.Replace("\n", "\r\n"), Template));
    }

    [Theory]
    [InlineData("app-sidebar")]
    [InlineData("dashboard-02")]
    [InlineData("auth-01")]
    public void OlderInstall_EditableBlockThatDiffersFromTheTemplate_HasLocalChanges(string name)
    {
        Assert.True(ComponentManager.HasLocalChanges(Entry(name, null), ComponentRegistry.GetMetadata(name)!, "edited", Template));
    }

    [Fact]
    public void OlderInstall_EditableBlockMatchingTheTemplate_HasNoLocalChanges()
    {
        Assert.False(ComponentManager.HasLocalChanges(Entry("app-sidebar", null), ComponentRegistry.GetMetadata("app-sidebar")!, Template, Template));
    }

    [Fact]
    public void OlderInstall_OrdinaryComponentIsUpdatedAsBefore()
    {
        Assert.False(ComponentManager.HasLocalChanges(Entry("button", null), ComponentRegistry.GetMetadata("button")!, "edited", Template));
    }
}
