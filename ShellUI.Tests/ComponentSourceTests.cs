using ShellUI.Templates;

namespace ShellUI.Tests;

public class ComponentSourceTests
{
    [Theory]
    [InlineData("Components/Kbd.razor", "@namespace YourProjectNamespace.Components.UI")]
    [InlineData("Variants/BadgeVariants.cs", "namespace YourProjectNamespace.Components.UI;")]
    [InlineData("Models/TabModels.cs", "namespace YourProjectNamespace.Components.Models;")]
    [InlineData("Components/Tabs.razor", "@using YourProjectNamespace.Components.Models")]
    [InlineData("Services/SonnerService.cs", "namespace YourProjectNamespace.Services;")]
    public void Read_SwapsThePackageNamespaceForThePlaceholder(string path, string expected)
    {
        var content = ComponentSource.Read(path);

        Assert.Contains(expected, content);
        Assert.DoesNotContain("ShellUI.Components", content);
    }

    [Fact]
    public void Read_PutsVariantsInTheirOwnNamespaceWhenAsked()
    {
        var variants = ComponentSource.Read("Variants/ButtonVariants.cs", variantsNamespace: true);
        var button = ComponentSource.Read("Components/Button.razor", variantsNamespace: true).Replace("\r\n", "\n");

        Assert.Contains("namespace YourProjectNamespace.Components.UI.Variants;", variants);
        Assert.StartsWith("@namespace YourProjectNamespace.Components.UI\n@using YourProjectNamespace.Components.UI.Variants\n", button);
    }

    [Fact]
    public void Read_EndsWithOneLineBreak()
    {
        var content = ComponentSource.Read("Components/Card.razor");

        Assert.EndsWith("}" + (content.Contains("\r\n") ? "\r\n" : "\n"), content);
    }

    [Fact]
    public void Read_FailsClearlyForAFileThatIsNotEmbedded()
    {
        var error = Assert.Throws<InvalidOperationException>(() => ComponentSource.Read("Components/Nope.razor"));

        Assert.Contains("Components/Nope.razor", error.Message);
    }

    [Theory]
    [InlineData("kbd")]
    [InlineData("badge")]
    [InlineData("badge-variants")]
    [InlineData("card")]
    [InlineData("card-footer")]
    public void Registry_ServesTemplatesReadFromThePackage(string name)
    {
        var content = ComponentRegistry.GetComponentContent(name);

        Assert.NotNull(content);
        Assert.Contains("YourProjectNamespace.Components.UI", content);
    }
}
