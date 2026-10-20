using ShellUI.CLI.Services;
using Xunit;

namespace ShellUI.Tests;

public class RazorClassLibraryTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), $"ShellUIRcl_{Guid.NewGuid():N}");

    public RazorClassLibraryTests() => Directory.CreateDirectory(_dir);

    public void Dispose() => Directory.Delete(_dir, true);

    [Fact]
    public void RazorClassLibrary_IsServedUnderItsName()
    {
        var project = Project("MyApp.UI", "Microsoft.NET.Sdk.Razor");

        Assert.Equal("_content/MyApp.UI", InitService.RazorClassLibraryAssetBase(project));
    }

    [Theory]
    [InlineData("<PackageId>Acme.Ui</PackageId>", "_content/Acme.Ui")]
    [InlineData("<AssemblyName>Acme.Components</AssemblyName>", "_content/Acme.Components")]
    [InlineData("<StaticWebAssetBasePath>/ui</StaticWebAssetBasePath>", "ui")]
    public void RazorClassLibrary_HonoursItsAssetName(string property, string expected)
    {
        var project = Project("MyApp.UI", "Microsoft.NET.Sdk.Razor", property);

        Assert.Equal(expected, InitService.RazorClassLibraryAssetBase(project));
    }

    [Fact]
    public void WebAppsAreNotLibraries()
    {
        Assert.Null(InitService.RazorClassLibraryAssetBase(Project("MyApp.Web", "Microsoft.NET.Sdk.Web")));

        var razorSdkWithProgram = Project("Hosted", "Microsoft.NET.Sdk.Razor");
        File.WriteAllText(Path.Combine(Path.GetDirectoryName(razorSdkWithProgram)!, "Program.cs"), "");
        Assert.Null(InitService.RazorClassLibraryAssetBase(razorSdkWithProgram));
    }

    [Fact]
    public void HostSteps_UseTheLibrarysAssetPath()
    {
        var steps = InitService.RazorClassLibraryHostSteps("_content/MyApp.UI");

        Assert.Contains(steps, s => s.Contains("_content/MyApp.UI/app.css") && s.Contains("_content/MyApp.UI/shellui.js"));
        Assert.Contains(steps, s => s.Contains("@source"));
        Assert.Contains(steps, s => s.Contains("prerender: false"));
        Assert.Contains(steps, s => s.Contains(InitService.RazorClassLibraryGuide));
    }

    [Fact]
    public void Guide_ShowsTheThemeScriptInitWrites()
    {
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (!File.Exists(Path.Combine(root.FullName, "ShellUI.slnx"))) root = root.Parent!;
        var guide = File.ReadAllText(Path.Combine(root.FullName, "docs", "razor-class-library.md")).Replace("\r\n", "\n");
        var appRazor = InitService.RewriteAppRazor("<html><head>\n</head><body></body></html>").Replace("\r\n", "\n");

        var start = appRazor.IndexOf("// ShellUI theme bootstrap", StringComparison.Ordinal);
        var script = appRazor[start..appRazor.IndexOf("</script>", start, StringComparison.Ordinal)];
        Assert.Contains(Unindent(script), Unindent(guide));
    }

    private static string Unindent(string text) =>
        string.Join("\n", text.Split('\n').Select(l => l.Trim()).Where(l => l.Length > 0));

    private string Project(string name, string sdk, string properties = "")
    {
        var dir = Path.Combine(_dir, name);
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, name + ".csproj");
        File.WriteAllText(path, $@"<Project Sdk=""{sdk}""><PropertyGroup>{properties}</PropertyGroup></Project>");
        return path;
    }
}
