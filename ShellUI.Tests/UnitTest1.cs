using ShellUI.Core.Models;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class ComponentRegistryTests
{
    [Fact]
    public void ComponentRegistry_ContainsExpectedComponents()
    {
        // Test that all components are registered
        Assert.True(ComponentRegistry.Exists("button"));
        Assert.True(ComponentRegistry.Exists("alert"));
        Assert.True(ComponentRegistry.Exists("badge"));
    }

    [Fact]
    public void ComponentRegistry_HasCorrectDependencies()
    {
        var buttonMeta = ComponentRegistry.GetMetadata("button");
        Assert.NotNull(buttonMeta);
        Assert.Contains("button-variants", buttonMeta.Dependencies);
    }
}

public class TemplateTests
{
    [Fact]
    public void ButtonTemplate_GeneratesCorrectContent()
    {
        var content = ComponentRegistry.GetComponentContent("button");
        Assert.NotNull(content);
        Assert.Contains("ButtonVariant", content);
        Assert.Contains("ButtonSize", content);
    }

    [Fact]
    public void TemplateContent_ContainsCorrectNamespace()
    {
        var content = ComponentRegistry.GetComponentContent("button");
        Assert.Contains("@namespace ShellUI.Components", content);
    }

    [Fact]
    public void TemplateContent_UsesShellUI_Namespace()
    {
        // Button template uses ShellUI.Components namespace in source
        var content = ComponentRegistry.GetComponentContent("button");
        Assert.Contains("@namespace ShellUI.Components", content);
    }
}