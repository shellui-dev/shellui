using ShellUI.CLI.Services;
using Xunit;

namespace ShellUI.Tests;

public class PackageVersionTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), $"ShellUIPackages_{Guid.NewGuid():N}");

    public PackageVersionTests() => Directory.CreateDirectory(_dir);

    public void Dispose() => Directory.Delete(_dir, true);

    [Theory]
    [InlineData("7.0.0", "6.0.2", true)]
    [InlineData("6.0.2", "6.0.2", true)]
    [InlineData("6.1", "6.0.2", true)]
    [InlineData("6.0.1", "6.0.2", false)]
    [InlineData("5.9.9", "6.0.2", false)]
    [InlineData("6.0.2-beta.1", "6.0.2", false)]
    [InlineData("6.0.2", "6.0.2-beta.1", true)]
    [InlineData("6.0.2-rc.1", "6.0.2-beta.1", true)]
    [InlineData("7.*", "6.0.2", true)]
    [InlineData("[6.0,8.0)", "6.0.2", true)]
    [InlineData("$(ApexVersion)", "6.0.2", true)]
    [InlineData("", "6.0.2", true)]
    public void SatisfiesMinimum_OnlyAddsOrRaises(string existing, string minimum, bool satisfied) =>
        Assert.Equal(satisfied, ComponentInstaller.SatisfiesMinimum(existing, minimum));

    [Fact]
    public void ReadsTheVersionAttribute_CaseInsensitively()
    {
        var project = Project(@"<PackageReference Include=""blazor-apexcharts"" Version=""7.0.0"" />");

        Assert.Equal("7.0.0", ComponentInstaller.ExistingPackageVersion(project, "Blazor-ApexCharts"));
        Assert.Null(ComponentInstaller.ExistingPackageVersion(project, "QRCoder"));
    }

    [Fact]
    public void ReadsAVersionElement()
    {
        var project = Project(@"<PackageReference Include=""QRCoder""><Version>1.9.0</Version></PackageReference>");

        Assert.Equal("1.9.0", ComponentInstaller.ExistingPackageVersion(project, "QRCoder"));
    }

    [Fact]
    public void ReadsCentralPackageManagement()
    {
        File.WriteAllText(Path.Combine(_dir, "Directory.Packages.props"),
            @"<Project><ItemGroup><PackageVersion Include=""Blazor-ApexCharts"" Version=""7.1.0"" /></ItemGroup></Project>");
        var project = Project(@"<PackageReference Include=""Blazor-ApexCharts"" />", subfolder: "src");

        Assert.Equal("7.1.0", ComponentInstaller.ExistingPackageVersion(project, "Blazor-ApexCharts"));
    }

    [Fact]
    public void ReferenceWithoutAFindableVersion_IsKeptAsIs()
    {
        var project = Project(@"<PackageReference Include=""QRCoder"" />");

        Assert.Equal("", ComponentInstaller.ExistingPackageVersion(project, "QRCoder"));
        Assert.True(ComponentInstaller.SatisfiesMinimum("", "1.8.0"));
    }

    private string Project(string references, string? subfolder = null)
    {
        var dir = subfolder == null ? _dir : Path.Combine(_dir, subfolder);
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "App.csproj");
        File.WriteAllText(path, $@"<Project Sdk=""Microsoft.NET.Sdk.Web""><ItemGroup>{references}</ItemGroup></Project>");
        return path;
    }
}
