using ShellUI.CLI.Services;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class AuthSetupTests
{
    private const string Layout = "MyApp.Components.Layout.AuthLayout01";

    [Theory]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void SetLayout_AddsTheLayoutWhereNoneIsSet(string newline)
    {
        var imports = $"@using MyApp.Components.Account.Shared{newline}@attribute [ExcludeFromInteractiveRouting]{newline}";

        var updated = AuthSetup.SetLayout(imports, Layout);

        Assert.Equal($"@using MyApp.Components.Account.Shared{newline}@attribute [ExcludeFromInteractiveRouting]{newline}@layout {Layout}{newline}", updated);
        Assert.Equal(updated, AuthSetup.SetLayout(updated!, Layout));
    }

    [Theory]
    [InlineData("@layout AccountLayout")]
    [InlineData("@layout MyApp.Components.Layout.AuthLayout02")]
    public void SetLayout_ReplacesTheNet8AccountLayoutOrAnotherAuthBlock(string directive)
    {
        var updated = AuthSetup.SetLayout($"@using MyApp.Components.Account.Shared\r\n{directive}\r\n", Layout);

        Assert.Equal($"@using MyApp.Components.Account.Shared\r\n@layout {Layout}\r\n", updated);
    }

    [Fact]
    public void SetLayout_LeavesACustomLayoutAlone()
    {
        Assert.Null(AuthSetup.SetLayout("@layout MyApp.Components.Layout.SignInLayout\n", Layout));
    }

    [Theory]
    [InlineData("Components/Layout", "MyApp.Components.Layout.AuthLayout01")]
    [InlineData("Layouts", "MyApp.Layouts.AuthLayout01")]
    public void LayoutTypeName_FollowsTheLayoutPath(string layoutPath, string expected)
    {
        Assert.Equal(expected, AuthSetup.LayoutTypeName("MyApp", layoutPath, "AuthLayout01"));
    }

    [Fact]
    public void EveryAuthBlock_IsAnInstallableLayout()
    {
        foreach (var (name, layout) in AuthSetup.Layouts)
        {
            var metadata = ComponentRegistry.GetMetadata(name);
            Assert.NotNull(metadata);
            Assert.True(metadata!.IsAvailable);
            Assert.True(metadata.IsLayoutBlock);
            Assert.Equal($"{layout}.razor", metadata.FilePath);
            Assert.Contains("@Body", ComponentRegistry.GetComponentContent(name));
        }
    }
}
