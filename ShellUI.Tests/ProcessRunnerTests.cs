using System.Diagnostics;
using ShellUI.CLI.Services;

namespace ShellUI.Tests;

public class ProcessRunnerTests
{
    [Fact]
    public async Task RunAsync_ReturnsLargeOutputWithoutDeadlocking()
    {
        // Far more than a pipe buffer, on both streams; waiting for exit before reading used to hang here.
        var startInfo = OperatingSystem.IsWindows()
            ? new ProcessStartInfo("cmd", "/c for /l %i in (1,1,6000) do @(echo out %i & echo err %i 1>&2)")
            : new ProcessStartInfo("sh", "-c \"for i in $(seq 1 6000); do echo out $i; echo err $i >&2; done\"");

        var result = await ProcessRunner.RunAsync(startInfo).WaitAsync(TimeSpan.FromSeconds(60));

        Assert.NotNull(result);
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("out 6000", result.Output);
        Assert.Contains("err 6000", result.Error);
    }

    [Fact]
    public void NodeTool_RunsNpmDirectlyOutsideWindows()
    {
        var startInfo = InitService.NodeTool(windows: false, "npx", "@tailwindcss/cli", "-i", "/my app/input.css");

        Assert.Equal("npx", startInfo.FileName);
        Assert.Equal(new[] { "@tailwindcss/cli", "-i", "/my app/input.css" }, startInfo.ArgumentList);
    }

    [Fact]
    public void NodeTool_GoesThroughCmdOnWindows_AndKeepsTheCaret()
    {
        var startInfo = InitService.NodeTool(windows: true, "npm", "install", "-D", "tailwindcss@^4.3.2", @"C:\my app\x");

        Assert.Equal("cmd", startInfo.FileName);
        Assert.Equal(@"/c npm install -D ""tailwindcss@^4.3.2"" ""C:\my app\x""", startInfo.Arguments);
    }
}
