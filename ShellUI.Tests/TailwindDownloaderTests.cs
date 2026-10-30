using System.Net;
using System.Net.Http.Headers;
using ShellUI.CLI.Services;
using Xunit;

namespace ShellUI.Tests;

public class TailwindDownloaderTests : IDisposable
{
    private const string Url = "https://example.test/tailwindcss";
    private readonly string _dir = Path.Combine(Path.GetTempPath(), $"ShellUITailwind_{Guid.NewGuid():N}");
    private readonly byte[] _binary = Enumerable.Range(0, 300_000).Select(i => (byte)(i * 7)).ToArray();

    private readonly string? _previousCache = Environment.GetEnvironmentVariable("SHELLUI_CACHE_DIR");

    private string Destination => Path.Combine(_dir, "tailwindcss-test");

    // Tests in a class run one at a time, so pointing the shared cache at a temp folder here can't affect the others.
    public TailwindDownloaderTests() => Environment.SetEnvironmentVariable("SHELLUI_CACHE_DIR", Path.Combine(_dir, "cache"));

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("SHELLUI_CACHE_DIR", _previousCache);
        if (Directory.Exists(_dir)) Directory.Delete(_dir, true);
    }

    [Fact]
    public async Task Downloads_TheWholeFile()
    {
        var server = new FakeServer((_, _) => Full(_binary));

        await TailwindDownloader.DownloadAsync(Url, Destination, handler: server, retryDelay: TimeSpan.Zero);

        Assert.Equal(_binary, File.ReadAllBytes(Destination));
        Assert.False(File.Exists(Destination + ".partial"));
    }

    [Fact]
    public async Task ResumesWithRange_AfterTheConnectionDrops()
    {
        var server = new FakeServer((request, attempt) => attempt == 1
            ? Full(_binary, new FailingStream(_binary[..100_000]))
            : Partial(_binary, request.Headers.Range!.Ranges.Single().From!.Value));

        await TailwindDownloader.DownloadAsync(Url, Destination, handler: server, retryDelay: TimeSpan.Zero);

        Assert.Equal(_binary, File.ReadAllBytes(Destination));
        Assert.Equal(100_000, server.Requests[1].Headers.Range!.Ranges.Single().From);
    }

    [Fact]
    public async Task RetriesAShortDownload_InsteadOfKeepingIt()
    {
        var server = new FakeServer((request, attempt) => attempt == 1
            ? Full(_binary, new MemoryStream(_binary[..50_000]))
            : Partial(_binary, request.Headers.Range!.Ranges.Single().From!.Value));

        await TailwindDownloader.DownloadAsync(Url, Destination, handler: server, retryDelay: TimeSpan.Zero);

        Assert.Equal(_binary, File.ReadAllBytes(Destination));
        Assert.Equal(2, server.Requests.Count);
    }

    [Fact]
    public async Task GivesUpOnAStall_AndRetries()
    {
        var server = new FakeServer((_, attempt) => attempt == 1
            ? Full(_binary, new StallingStream(_binary[..1000]))
            : Full(_binary));

        await TailwindDownloader.DownloadAsync(Url, Destination, handler: server,
            stallTimeout: TimeSpan.FromMilliseconds(300), retryDelay: TimeSpan.Zero);

        Assert.Equal(_binary, File.ReadAllBytes(Destination));
    }

    [Fact]
    public async Task StartsOver_WhenTheServerIgnoresRange()
    {
        var server = new FakeServer((_, attempt) => attempt == 1
            ? Full(_binary, new FailingStream(_binary[..100_000]))
            : Full(_binary));

        await TailwindDownloader.DownloadAsync(Url, Destination, handler: server, retryDelay: TimeSpan.Zero);

        Assert.Equal(_binary, File.ReadAllBytes(Destination));
    }

    [Fact]
    public async Task NotFound_FailsAtOnce_AndLeavesNoFile()
    {
        var server = new FakeServer((_, _) => new HttpResponseMessage(HttpStatusCode.NotFound));

        await Assert.ThrowsAsync<IOException>(() =>
            TailwindDownloader.DownloadAsync(Url, Destination, handler: server, retryDelay: TimeSpan.Zero));

        Assert.Single(server.Requests);
        Assert.False(File.Exists(Destination));
    }

    [Fact]
    public async Task KeepsFailing_ThrowsAfterTheLastAttempt_AndLeavesNoFile()
    {
        var server = new FakeServer((_, _) => Full(_binary, new FailingStream(_binary[..10])));

        await Assert.ThrowsAsync<IOException>(() =>
            TailwindDownloader.DownloadAsync(Url, Destination, handler: server, retryDelay: TimeSpan.Zero));

        Assert.Equal(4, server.Requests.Count);
        Assert.False(File.Exists(Destination));
    }

    [Fact]
    public void CachePath_MatchesThePrecompiledCssScript()
    {
        var name = Path.GetFileName(TailwindDownloader.CachedPath());

        Assert.StartsWith($"tailwindcss-{ShellUI.Core.TailwindConstants.Version}-", name);
    }

    [Fact]
    public void BuildTargets_UseTheSameCacheName()
    {
        var targets = InitService.GetTargetsFileContent("standalone");

        Assert.Contains($"<TailwindVersion>{ShellUI.Core.TailwindConstants.Version}</TailwindVersion>", targets);
        Assert.Contains("tailwindcss-$(TailwindVersion)-$(TailwindPlatform)", targets);
        Assert.Contains("<TailwindExecutable Condition=\"'$(TailwindExecutable)' == ''\">$(TailwindCached)</TailwindExecutable>", targets);
        Assert.DoesNotContain("<Copy ", targets);
        Assert.True(targets.IndexOf("DownloadTailwindCLI") < targets.IndexOf("WarnMissingTailwindCLI")
                    && targets.IndexOf("WarnMissingTailwindCLI") < targets.IndexOf("BuildTailwindCSS"));
    }

    [Theory]
    [InlineData("ShellUI-0.3.4.targets.txt")]
    [InlineData("ShellUI-0.4.0-alpha.1.targets.txt")]
    [InlineData("ShellUI-0.4.0-alpha.3.targets.txt")]
    public void Update_ReplacesATargetsFileAnEarlierCliWrote(string fixture)
    {
        var path = WriteTargets(Fixture(fixture));

        var notes = InitService.RefreshTailwindSetup(_dir);

        Assert.Contains(notes, n => n.Contains("Updated:"));
        Assert.Equal(InitService.GetTargetsFileContent("standalone"), File.ReadAllText(path));
        Assert.Equal("*\n", File.ReadAllText(Path.Combine(_dir, ".shellui", ".gitignore")));
    }

    [Fact]
    public void Update_LeavesAnEditedTargetsFile_AndSaysSo()
    {
        var edited = Fixture("ShellUI-0.4.0-alpha.1.targets.txt").Replace("--minify", "--minify --optimize");
        var path = WriteTargets(edited);

        var notes = InitService.RefreshTailwindSetup(_dir);

        Assert.Contains(notes, n => n.Contains("local changes"));
        Assert.Equal(edited, File.ReadAllText(path));
    }

    [Fact]
    public void Update_LeavesCurrentAndNpmTargetsAlone()
    {
        WriteTargets(InitService.GetTargetsFileContent("standalone"));
        Assert.Empty(InitService.RefreshTailwindSetup(_dir));

        var npm = WriteTargets(InitService.GetTargetsFileContent("npm"));
        Assert.Empty(InitService.RefreshTailwindSetup(_dir));
        Assert.Equal(InitService.GetTargetsFileContent("npm"), File.ReadAllText(npm));
    }

    [Fact]
    public void Update_DropsTheProjectCopy_WhenItIsTheSharedOne()
    {
        WriteTargets(Fixture("ShellUI-0.4.0-alpha.1.targets.txt"));
        var cached = TailwindDownloader.CachedPath();
        Directory.CreateDirectory(Path.GetDirectoryName(cached)!);
        File.WriteAllBytes(cached, _binary);
        var projectCopy = TailwindDownloader.GetTailwindPath(_dir);
        Directory.CreateDirectory(Path.GetDirectoryName(projectCopy)!);
        File.WriteAllBytes(projectCopy, _binary);

        var notes = InitService.RefreshTailwindSetup(_dir);

        Assert.Contains(notes, n => n.Contains("Removed:"));
        Assert.False(File.Exists(projectCopy));
        Assert.True(File.Exists(cached));
    }

    [Fact]
    public void Update_KeepsAProjectCopyThatDiffersFromTheSharedOne()
    {
        WriteTargets(InitService.GetTargetsFileContent("standalone"));
        var cached = TailwindDownloader.CachedPath();
        Directory.CreateDirectory(Path.GetDirectoryName(cached)!);
        File.WriteAllBytes(cached, _binary);
        var projectCopy = TailwindDownloader.GetTailwindPath(_dir);
        Directory.CreateDirectory(Path.GetDirectoryName(projectCopy)!);
        File.WriteAllBytes(projectCopy, _binary[..1000]);

        InitService.RefreshTailwindSetup(_dir);

        Assert.True(File.Exists(projectCopy));
    }

    [Fact]
    public void ShellUiFolder_IgnoresItself_AndLeavesAnExistingIgnoreAlone()
    {
        TailwindDownloader.IgnoreShellUiFolder(_dir);
        var ignore = Path.Combine(_dir, ".shellui", ".gitignore");
        Assert.Equal("*\n", File.ReadAllText(ignore));

        File.WriteAllText(ignore, "bin/\n");
        TailwindDownloader.IgnoreShellUiFolder(_dir);
        Assert.Equal("bin/\n", File.ReadAllText(ignore));
    }

    private string WriteTargets(string content)
    {
        var path = Path.Combine(_dir, "Build", "ShellUI.targets");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content);
        return path;
    }

    private static string Fixture(string name, [System.Runtime.CompilerServices.CallerFilePath] string thisFile = "") =>
        File.ReadAllText(Path.Combine(Path.GetDirectoryName(thisFile)!, "Fixtures", "Targets", name));

    private static HttpResponseMessage Full(byte[] body, Stream? stream = null)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(stream ?? new MemoryStream(body)) };
        response.Content.Headers.ContentLength = body.Length;
        return response;
    }

    private static HttpResponseMessage Partial(byte[] body, long from)
    {
        var response = new HttpResponseMessage(HttpStatusCode.PartialContent) { Content = new ByteArrayContent(body[(int)from..]) };
        response.Content.Headers.ContentRange = new ContentRangeHeaderValue(from, body.Length - 1, body.Length);
        return response;
    }

    private sealed class FakeServer : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, int, HttpResponseMessage> _respond;
        public List<HttpRequestMessage> Requests { get; } = new();

        public FakeServer(Func<HttpRequestMessage, int, HttpResponseMessage> respond) => _respond = respond;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            return Task.FromResult(_respond(request, Requests.Count));
        }
    }

    // Sends its bytes, then fails like a dropped connection.
    private sealed class FailingStream(byte[] data) : MemoryStream(data)
    {
        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = base.Read(buffer, offset, count);
            return read > 0 ? read : throw new IOException("connection reset");
        }

        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            var read = await base.ReadAsync(buffer, cancellationToken);
            return read > 0 ? read : throw new IOException("connection reset");
        }
    }

    // Sends its bytes, then never sends anything again.
    private sealed class StallingStream(byte[] data) : MemoryStream(data)
    {
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            var read = await base.ReadAsync(buffer, cancellationToken);
            if (read > 0) return read;
            await Task.Delay(Timeout.Infinite, cancellationToken);
            return 0;
        }
    }
}
