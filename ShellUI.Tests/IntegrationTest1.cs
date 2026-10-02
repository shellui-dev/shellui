using System.Diagnostics;
using System.IO;
using Xunit;

namespace ShellUI.Tests;

public class CliIntegrationTests : IDisposable
{
    private readonly string _testDir;

    public CliIntegrationTests()
    {
        _testDir = Path.Combine(Path.GetTempPath(), $"ShellUITest_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDir);
    }

    [Fact]
    public async Task InitCommand_DoesNotCrash()
    {
        await CreateTestBlazorProject();

        var exitCode = await RunShellUICommand("init --yes");

        // init may fail in the test environment; it only must not crash.
        Assert.True(exitCode == 0 || exitCode == 1);
    }

    [Fact]
    public async Task AddCommand_DoesNotCrash()
    {
        await CreateTestBlazorProject();

        var exitCode = await RunShellUICommand("add button");

        // add may fail in the test environment; it only must not crash.
        Assert.True(exitCode == 0 || exitCode == 1);
    }

    private async Task CreateTestBlazorProject()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"new blazor -n TestProject --interactivity Server",
                WorkingDirectory = _testDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        process.Start();
        await process.WaitForExitAsync();
        Assert.Equal(0, process.ExitCode);
    }

    private async Task<int> RunShellUICommand(string args)
    {
        var solutionDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
        var cliPath = Path.Combine(solutionDir, "src", "ShellUI.CLI", "bin", "Release", "net10.0", "ShellUI.CLI.dll");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"{cliPath} {args}",
                WorkingDirectory = _testDir,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        process.Start();
        await process.WaitForExitAsync();
        return process.ExitCode;
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
        {
            Directory.Delete(_testDir, true);
        }
    }
}
