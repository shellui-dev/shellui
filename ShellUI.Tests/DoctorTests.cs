using System.Text.Json;
using ShellUI.CLI.Services;
using ShellUI.Core.Models;

namespace ShellUI.Tests;

public class DoctorTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), $"doctor-{Guid.NewGuid():N}");

    public DoctorTests()
    {
        Directory.CreateDirectory(Path.Combine(_root, "Components", "UI", "Variants"));
        Directory.CreateDirectory(Path.Combine(_root, "wwwroot"));
        Directory.CreateDirectory(Path.Combine(_root, "Build"));
        File.WriteAllText(Path.Combine(_root, "MyApp.csproj"), "<Project Sdk=\"Microsoft.NET.Sdk.Web\"></Project>");
        File.WriteAllText(Path.Combine(_root, "Build", "ShellUI.targets"), "<Project />");
        File.WriteAllText(Path.Combine(_root, "Components", "UI", "Button.razor"), "");
        File.WriteAllText(Path.Combine(_root, "Components", "UI", "Variants", "ButtonVariants.cs"), "");
        File.WriteAllText(Path.Combine(_root, "Components", "_Imports.razor"), "@using MyApp.Components.UI\n@using MyApp.Components.UI.Variants\n");
        File.WriteAllText(Path.Combine(_root, "Components", "App.razor"),
            "<head><link rel=\"stylesheet\" href=\"app.css\" /><script>// ShellUI theme bootstrap</script><HeadOutlet @rendermode=\"InteractiveServer\" /></head>\n" +
            "<body><Routes @rendermode=\"InteractiveServer\" /><script src=\"shellui.js\"></script></body>");
        File.WriteAllText(Path.Combine(_root, "wwwroot", "input.css"), "@import \"tailwindcss\";");
        File.WriteAllText(Path.Combine(_root, "wwwroot", "app.css"), "");
        File.SetLastWriteTimeUtc(Path.Combine(_root, "wwwroot", "app.css"), DateTime.UtcNow.AddMinutes(1));
        WriteConfig("button", "button-variants");
    }

    public void Dispose()
    {
        try { Directory.Delete(_root, recursive: true); } catch (IOException) { }
    }

    private void WriteConfig(params string[] installed)
    {
        var config = new ShellUIConfig { InstalledComponents = installed.Select(n => new InstalledComponent { Name = n, Version = "0.4.0" }).ToList() };
        File.WriteAllText(Path.Combine(_root, "shellui.json"), JsonSerializer.Serialize(config));
    }

    private List<DoctorFinding> Errors() => DoctorService.Diagnose(_root).Where(f => f.Severity == DoctorSeverity.Error).ToList();

    [Fact]
    public void HealthyProject_HasNoErrors()
    {
        Assert.Empty(Errors());
    }

    [Fact]
    public void MissingShelluiJson_IsReported()
    {
        File.Delete(Path.Combine(_root, "shellui.json"));

        Assert.Contains(Errors(), f => f.Message.Contains("shellui.json"));
    }

    [Fact]
    public void NoRenderMode_IsReported()
    {
        var app = Path.Combine(_root, "Components", "App.razor");
        File.WriteAllText(app, File.ReadAllText(app).Replace(" @rendermode=\"InteractiveServer\"", ""));

        Assert.Contains(Errors(), f => f.Message.Contains("render mode"));
    }

    [Fact]
    public void HostPageWithoutShelluiJs_IsReported()
    {
        var app = Path.Combine(_root, "Components", "App.razor");
        File.WriteAllText(app, File.ReadAllText(app).Replace("<script src=\"shellui.js\"></script>", ""));

        Assert.Contains(Errors(), f => f.Message.Contains("shellui.js"));
    }

    [Fact]
    public void MissingDependency_IsReported()
    {
        WriteConfig("button");

        Assert.Contains(Errors(), f => f.Message.Contains("button-variants"));
    }

    [Fact]
    public void DeletedComponentFile_IsReported()
    {
        File.Delete(Path.Combine(_root, "Components", "UI", "Button.razor"));

        Assert.Contains(Errors(), f => f.Message.Contains("file is gone") && f.Message.Contains("button"));
    }

    [Fact]
    public void MissingUsing_IsReported()
    {
        File.WriteAllText(Path.Combine(_root, "Components", "_Imports.razor"), "@using MyApp.Components.UI\n");

        Assert.Contains(Errors(), f => f.Message.Contains("MyApp.Components.UI.Variants"));
    }

    [Fact]
    public void PackageAndCliTogether_IsAWarning()
    {
        File.WriteAllText(Path.Combine(_root, "MyApp.csproj"),
            "<Project Sdk=\"Microsoft.NET.Sdk.Web\"><ItemGroup><PackageReference Include=\"ShellUI.Components\" Version=\"0.4.0\" /></ItemGroup></Project>");

        Assert.Contains(DoctorService.Diagnose(_root), f => f.Severity == DoctorSeverity.Warning && f.Message.Contains("ShellUI.Components package"));
    }

    [Fact]
    public void StaleAppCss_IsAWarning()
    {
        File.SetLastWriteTimeUtc(Path.Combine(_root, "wwwroot", "app.css"), DateTime.UtcNow.AddHours(-1));

        Assert.Contains(DoctorService.Diagnose(_root), f => f.Severity == DoctorSeverity.Warning && f.Message.Contains("older than"));
    }
}
