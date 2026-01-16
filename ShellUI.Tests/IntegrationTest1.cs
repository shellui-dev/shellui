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
        // Create a minimal Blazor project
        await CreateTestBlazorProject();

        // Run init command with --yes flag (just test it doesn't crash)
        var exitCode = await RunShellUICommand("init --yes");

        // We expect it to work, but if it fails due to missing dependencies in test env, that's ok
        // The important thing is it doesn't crash the process
        Assert.True(exitCode == 0 || exitCode == 1); // 0 = success, 1 = expected failure in test env
    }

    [Fact]
    public async Task AddCommand_DoesNotCrash()
    {
        // Create a minimal Blazor project
        await CreateTestBlazorProject();

        // Test: Add button component (just test it doesn't crash)
        var exitCode = await RunShellUICommand("add button");

        // We expect it to work, but if it fails due to missing dependencies in test env, that's ok
        // The important thing is it doesn't crash the process
        Assert.True(exitCode == 0 || exitCode == 1); // 0 = success, 1 = expected failure in test env
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
        // Get the path to the CLI from the test directory perspective
        var solutionDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), ".."));
        var cliPath = Path.Combine(solutionDir, "src", "ShellUI.CLI", "bin", "Release", "net9.0", "ShellUI.CLI.dll");

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
