using System.Diagnostics;

namespace ShellUI.CLI.Services;

public record ProcessResult(int ExitCode, string Output, string Error);

public static class ProcessRunner
{
    // Reads both streams while the process runs; waiting for exit first deadlocks once output fills the pipe buffer.
    public static async Task<ProcessResult?> RunAsync(ProcessStartInfo startInfo)
    {
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        using var process = Process.Start(startInfo);
        if (process == null) return null;

        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        return new ProcessResult(process.ExitCode, await output, await error);
    }
}
