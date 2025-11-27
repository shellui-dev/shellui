using System.IO.Compression;
using System.Runtime.InteropServices;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class TailwindDownloader
{
    private const string TailwindVersion = "v4.1.14";
    private const string BaseUrl = "https://github.com/tailwindlabs/tailwindcss/releases/download";
    
    public static async Task<string> EnsureTailwindCliAsync(string projectRoot)
    {
        var binPath = Path.Combine(projectRoot, ".shellui", "bin");
        Directory.CreateDirectory(binPath);

        var executableName = GetExecutableName();
        var tailwindPath = Path.Combine(binPath, executableName);

        if (File.Exists(tailwindPath))
        {
            AnsiConsole.MarkupLine("[dim]Tailwind CLI already installed[/]");
            return tailwindPath;
        }

        await DownloadTailwindCliAsync(tailwindPath);
        return tailwindPath;
    }

    private static async Task DownloadTailwindCliAsync(string destinationPath)
    {
        var platform = GetPlatformIdentifier();
        var downloadUrl = $"{BaseUrl}/{TailwindVersion}/tailwindcss-{platform}";

        AnsiConsole.MarkupLine($"[cyan]Downloading Tailwind CLI {TailwindVersion}...[/]");
        
        await AnsiConsole.Status()
            .StartAsync("Downloading...", async ctx =>
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromMinutes(5);

                var response = await client.GetAsync(downloadUrl);
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to download Tailwind CLI: {response.StatusCode}");
                }

                var bytes = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(destinationPath, bytes);

                // Make executable on Unix systems
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var process = System.Diagnostics.Process.Start("chmod", $"+x {destinationPath}");
                    process?.WaitForExit();
                }
            });

        AnsiConsole.MarkupLine("[green]Tailwind CLI downloaded successfully![/]");
    }

    private static string GetPlatformIdentifier()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => "windows-x64.exe",
                Architecture.Arm64 => "windows-arm64.exe",
                _ => throw new PlatformNotSupportedException("Unsupported Windows architecture")
            };
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => "linux-x64",
                Architecture.Arm64 => "linux-arm64",
                Architecture.Arm => "linux-armv7",
                _ => throw new PlatformNotSupportedException("Unsupported Linux architecture")
            };
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => "macos-x64",
                Architecture.Arm64 => "macos-arm64",
                _ => throw new PlatformNotSupportedException("Unsupported macOS architecture")
            };
        }

        throw new PlatformNotSupportedException("Unsupported operating system");
    }

    private static string GetExecutableName()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
            ? "tailwindcss.exe" 
            : "tailwindcss";
    }

    public static string GetTailwindPath(string projectRoot)
    {
        var binPath = Path.Combine(projectRoot, ".shellui", "bin");
        var executableName = GetExecutableName();
        return Path.Combine(binPath, executableName);
    }
}

