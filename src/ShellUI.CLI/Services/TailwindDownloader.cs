using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using ShellUI.Core;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class TailwindDownloader
{
    private const string TailwindVersion = TailwindConstants.GitHubTag;
    private const string BaseUrl = "https://github.com/tailwindlabs/tailwindcss/releases/download";
    private const int Attempts = 4;
    private static readonly TimeSpan StallTimeout = TimeSpan.FromSeconds(60);

    // The CLI to run: a project's own copy in .shellui/bin if it has one, otherwise the shared per-machine copy,
    // downloaded on first use. Projects hold no binary. Returns null when it can't be had, so init can carry on.
    public static async Task<string?> EnsureTailwindCliAsync(string projectRoot, Action<string>? progress = null)
    {
        IgnoreShellUiFolder(projectRoot);
        var projectCopy = GetTailwindPath(projectRoot);
        if (File.Exists(projectCopy)) return projectCopy;

        var cached = CachedPath();
        if (!File.Exists(cached))
        {
            var url = $"{BaseUrl}/{TailwindVersion}/tailwindcss-{GetPlatformIdentifier()}";
            AnsiConsole.MarkupLine($"[cyan]Downloading Tailwind CLI {TailwindVersion}[/] [dim]to {Markup.Escape(cached)}[/]");
            try
            {
                await DownloadAsync(url, cached, progress);
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[yellow]Warning:[/] Could not download the Tailwind CLI: {Markup.Escape(ex.Message)}");
                AnsiConsole.MarkupLine($"[dim]The build retries the download. To install it by hand, save[/] {Markup.Escape(url)} [dim]as[/] {Markup.Escape(cached)}");
                return null;
            }
            AnsiConsole.MarkupLine("[green]Tailwind CLI downloaded[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[dim]Using the Tailwind CLI in {Markup.Escape(CacheDirectory())}[/]");
        }
        return cached;
    }

    // A .gitignore of "*" inside .shellui keeps the folder (and any binary in it) out of git without touching the project's own .gitignore.
    public static void IgnoreShellUiFolder(string projectRoot)
    {
        var ignore = Path.Combine(projectRoot, ".shellui", ".gitignore");
        if (File.Exists(ignore)) return;
        Directory.CreateDirectory(Path.GetDirectoryName(ignore)!);
        File.WriteAllText(ignore, "*\n");
    }

    // Projects set up before the shared cache carry their own copy; drop it when it is the same file as the shared one.
    public static bool RemoveDuplicateProjectCopy(string projectRoot)
    {
        var projectCopy = GetTailwindPath(projectRoot);
        var cached = CachedPath();
        if (!File.Exists(projectCopy) || !File.Exists(cached)) return false;
        if (new FileInfo(projectCopy).Length != new FileInfo(cached).Length) return false;

        using (var a = File.OpenRead(projectCopy))
        using (var b = File.OpenRead(cached))
        {
            if (!System.Security.Cryptography.SHA256.HashData(a).AsSpan().SequenceEqual(System.Security.Cryptography.SHA256.HashData(b)))
                return false;
        }
        File.Delete(projectCopy);
        return true;
    }

    // Streams to <destination>.partial and resumes it on retry; the file only appears once its size matches.
    internal static async Task DownloadAsync(string url, string destination, Action<string>? progress = null,
        HttpMessageHandler? handler = null, TimeSpan? stallTimeout = null, TimeSpan? retryDelay = null)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        var partial = destination + ".partial";
        using var client = handler == null ? new HttpClient() : new HttpClient(handler, disposeHandler: false);
        client.Timeout = Timeout.InfiniteTimeSpan;
        client.DefaultRequestHeaders.UserAgent.ParseAdd("ShellUI-CLI");

        Exception? last = null;
        for (var attempt = 1; attempt <= Attempts; attempt++)
        {
            // Another init may have finished the same download meanwhile.
            if (attempt > 1 && File.Exists(destination)) return;
            try
            {
                await DownloadOnceAsync(client, url, partial, progress, stallTimeout ?? StallTimeout);
                File.Move(partial, destination, overwrite: true);
                MakeExecutable(destination);
                return;
            }
            catch (HttpRequestException ex) when (ex.StatusCode is HttpStatusCode code && (int)code is >= 400 and < 500
                                                   && code is not (HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests))
            {
                throw new IOException($"the server answered {(int)code} {code}", ex);
            }
            catch (Exception ex) when (ex is HttpRequestException or IOException or OperationCanceledException)
            {
                last = ex;
                if (attempt == Attempts) break;
                progress?.Invoke($"Tailwind CLI download interrupted, retrying ({attempt}/{Attempts - 1})...");
                await Task.Delay(retryDelay ?? TimeSpan.FromSeconds(2 * attempt));
            }
        }

        throw new IOException($"gave up after {Attempts} attempts ({last?.Message})", last);
    }

    private static async Task DownloadOnceAsync(HttpClient client, string url, string partial, Action<string>? progress, TimeSpan stallTimeout)
    {
        var existing = File.Exists(partial) ? new FileInfo(partial).Length : 0;
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (existing > 0) request.Headers.Range = new RangeHeaderValue(existing, null);

        using var stall = new CancellationTokenSource(stallTimeout);
        using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, stall.Token);

        if (response.StatusCode == HttpStatusCode.RequestedRangeNotSatisfiable)
        {
            File.Delete(partial);
            throw new IOException("partial download was invalid; starting over");
        }
        response.EnsureSuccessStatusCode();

        // A server that ignores Range sends the whole file again.
        var resuming = existing > 0 && response.StatusCode == HttpStatusCode.PartialContent;
        var total = response.Content.Headers.ContentLength is long length ? length + (resuming ? existing : 0) : (long?)null;
        var written = resuming ? existing : 0;

        await using (var source = await response.Content.ReadAsStreamAsync(stall.Token))
        await using (var target = new FileStream(partial, resuming ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var buffer = new byte[81920];
            var lastReport = DateTime.MinValue;
            // Without a terminal each update is a log line, so report every 10% instead of continuously.
            var step = AnsiConsole.Profile.Capabilities.Interactive ? 1 : 10;
            var lastStep = -1L;
            int read;
            while ((read = await source.ReadAsync(buffer, stall.Token)) > 0)
            {
                stall.CancelAfter(stallTimeout);
                await target.WriteAsync(buffer.AsMemory(0, read));
                written += read;
                if (progress == null || DateTime.UtcNow - lastReport < TimeSpan.FromMilliseconds(250)) continue;

                var reached = total is long t && t > 0 ? written * 100 / t / step : written / 1048576 / (step * 2);
                if (reached == lastStep) continue;
                lastStep = reached;
                lastReport = DateTime.UtcNow;
                progress(total is long size && size > 0
                    ? $"Downloading Tailwind CLI... {written * 100 / size}% ({written / 1048576} of {size / 1048576} MB)"
                    : $"Downloading Tailwind CLI... {written / 1048576} MB");
            }
        }

        if (total is long expected && written != expected)
            throw new IOException($"download ended at {written} of {expected} bytes");
    }

    // The same location and name scripts/rebuild-precompiled-css.sh uses, so one copy serves every project.
    public static string CacheDirectory() =>
        Environment.GetEnvironmentVariable("SHELLUI_CACHE_DIR") is { Length: > 0 } dir
            ? dir
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".shellui", "bin");

    public static string CachedPath() =>
        Path.Combine(CacheDirectory(), $"tailwindcss-{TailwindConstants.Version}-{GetPlatformIdentifier()}");

    private static void MakeExecutable(string path)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
        File.SetUnixFileMode(path, File.GetUnixFileMode(path) | UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
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
