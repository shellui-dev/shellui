using ShellUI.Core;
using ShellUI.Core.Models;
using ShellUI.Templates;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class InitService
{
    public static async Task InitializeAsync(string style, bool force, string? tailwindMethod = null, bool nonInteractive = false,
        string? dashboard = null, LayoutSwitch layoutSwitch = LayoutSwitch.Ask)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");
        var dashboardChoice = DashboardSetup.ParseDashboardOption(dashboard) ?? (nonInteractive ? "none" : null);
        var tailwindChoice = tailwindMethod?.Trim().ToLowerInvariant();
        if (tailwindChoice is not (null or "standalone" or "npm"))
            throw new ArgumentException($"Unknown --tailwind value '{tailwindMethod}'. Use standalone or npm.");

        if (File.Exists(configPath) && !force)
        {
            AnsiConsole.MarkupLine("[yellow]ShellUI is already initialized in this project.[/]");
            AnsiConsole.MarkupLine("[dim]Use --force to reinitialize, or add a dashboard with: shellui add dashboard-02[/]");
            return;
        }

        var projectInfo = ProjectDetector.DetectProject();
        AnsiConsole.MarkupLine($"[green]✅ Detected:[/] {projectInfo.ProjectType}");
        AnsiConsole.MarkupLine($"[dim]Project: {projectInfo.ProjectName}[/]");
        AnsiConsole.MarkupLine($"[dim]Namespace: {projectInfo.RootNamespace}[/]");
        RemoveBootstrapFiles();

        AnsiConsole.MarkupLine("[cyan]Setting up Tailwind CSS...[/]");
        string method;

        if (tailwindChoice != null)
        {
            method = tailwindChoice;
            AnsiConsole.MarkupLine($"[green]✅ Tailwind:[/] {method}");
        }
        else if (nonInteractive)
        {
            method = "standalone";
            AnsiConsole.MarkupLine("[green]✅ Tailwind:[/] standalone [dim](default with --yes; pass --tailwind npm to use npm)[/]");
        }
        else
        {
            var tailwindSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Which Tailwind CSS method do you prefer?[/]")
                    .PageSize(3)
                    .AddChoices(new[] {
                        "standalone - Use Tailwind CLI (no Node.js required, automatic builds)",
                        "npm - Use npm packages (requires Node.js, more flexible)"
                    }));

            method = tailwindSelection.StartsWith("standalone") ? "standalone" : "npm";
            AnsiConsole.MarkupLine($"[green]✅ Selected:[/] {method}");
        }

        if (dashboardChoice == null)
        {
            var dashboardSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Set up the ShellUI dashboard layout?[/] [dim](sidebar, header, theme toggle)[/]")
                    .AddChoices(new[] {
                        "Sticky header (dashboard-02, recommended)",
                        "Scrolling header (dashboard-01)",
                        "No, keep my layout"
                    }));

            dashboardChoice = dashboardSelection.StartsWith("Sticky") ? "dashboard-02"
                : dashboardSelection.StartsWith("Scrolling") ? "dashboard-01"
                : "none";
            AnsiConsole.MarkupLine($"[green]✅ Selected:[/] {dashboardChoice}");
        }

        if (method == "npm" && !await IsNpmAvailableAsync())
        {
            AnsiConsole.MarkupLine("[red]Error: npm is not available. Please install Node.js and npm, or choose the 'standalone' Tailwind method.[/]");
            return;
        }

        var config = new ShellUIConfig
        {
            Style = style,
            ComponentsPath = "Components/UI",
            LayoutPath = "Components/Layout",
            ProjectType = projectInfo.ProjectType,
            Tailwind = new TailwindConfig
            {
                Enabled = true,
                Version = TailwindConstants.Version,
                Method = method,
                CssPath = "wwwroot/app.css"
            }
        };

        string? tailwindPath = null;
        await LogoLoader.RunAsync("Installing ShellUI components...", async status =>
            {
                status("Creating component folders...");
                var componentsPath = Path.Combine(Directory.GetCurrentDirectory(), "Components", "UI");
                Directory.CreateDirectory(componentsPath);
                AnsiConsole.MarkupLine($"[green]Created:[/] Components/UI/");

                status("Installing Shell utilities...");
                if (!await ComponentInstaller.InstallComponentForInitAsync("shell", projectInfo, config))
                    throw new InvalidOperationException("Shell could not be installed.");
                if (!await ComponentInstaller.InstallComponentForInitAsync("shellui-js", projectInfo, config))
                    throw new InvalidOperationException("shellui.js could not be installed.");

                status("Preparing configuration...");
                AnsiConsole.MarkupLine("[green]Configuration prepared[/]");

                status("Setting up imports...");
                // Razor class libraries keep _Imports.razor at the project root.
                var importsPath = new[] { Path.Combine("Components", "_Imports.razor"), "_Imports.razor" }
                    .Select(p => Path.Combine(Directory.GetCurrentDirectory(), p))
                    .FirstOrDefault(File.Exists);
                if (importsPath != null)
                {
                    var importsContent = File.ReadAllText(importsPath);
                    var usingStatement = $"@using {projectInfo.RootNamespace}.Components.UI";

                    if (!importsContent.Contains(usingStatement))
                    {
                        File.AppendAllText(importsPath, $"\n{usingStatement}\n");
                        AnsiConsole.MarkupLine($"[green]Updated:[/] {Path.GetRelativePath(Directory.GetCurrentDirectory(), importsPath)}");
                    }
                }

                status("Setting up Tailwind CSS...");
                if (method == "npm")
                {
                    await SetupTailwindNpmAsync();
                }
                else
                {
                    tailwindPath = await SetupTailwindStandaloneAsync(status);
                }

                status("Wiring up theme and render mode...");
                await BootstrapHostAsync(projectInfo);

                status("Restyling the template's sample pages...");
                var pages = StockPages.Apply(Directory.GetCurrentDirectory(), new[] { config.ComponentsPath, config.LayoutPath });
                if (pages.Restyled.Count > 0)
                    AnsiConsole.MarkupLine($"[green]Restyled:[/] {Markup.Escape(string.Join(", ", pages.Restyled))} [dim](Bootstrap classes → Tailwind)[/]");
                foreach (var note in pages.Notes)
                    AnsiConsole.MarkupLine($"[yellow]![/] {Markup.Escape(note)}");

                if (DashboardSetup.IsIdentityApp(Directory.GetCurrentDirectory()))
                    AnsiConsole.MarkupLine($"[yellow]![/] {Markup.Escape(AuthSetup.Hint)}");

                status("Setting up MSBuild integration...");
                var buildPath = Path.Combine(Directory.GetCurrentDirectory(), "Build");
                Directory.CreateDirectory(buildPath);

                var targetsPath = Path.Combine(buildPath, "ShellUI.targets");
                var targetsContent = GetTargetsFileContent(method);
                File.WriteAllText(targetsPath, targetsContent);
                AnsiConsole.MarkupLine($"[green]Created:[/] Build/ShellUI.targets");

                await UpdateProjectFileAsync(projectInfo.ProjectPath, targetsPath);
                AnsiConsole.MarkupLine($"[green]Updated:[/] {Path.GetFileName(projectInfo.ProjectPath)}");

                status("Building Tailwind CSS...");
                if (method == "npm")
                {
                    await RunNpmTailwindBuildAsync();
                    AnsiConsole.MarkupLine($"[green]Built:[/] Tailwind CSS");
                }
                else if (tailwindPath != null)
                {
                    var inputCssPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "input.css");
                    var appCssPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app.css");
                    await RunTailwindBuildAsync(tailwindPath, inputCssPath, appCssPath);
                    AnsiConsole.MarkupLine($"[green]Built:[/] Tailwind CSS");
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Skipped:[/] Tailwind CSS build [dim](no Tailwind CLI yet; dotnet build tries the download again)[/]");
                }
            });

        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(configPath, json);
        AnsiConsole.MarkupLine("[green]✅ Created:[/] shellui.json");

        if (dashboardChoice != "none")
        {
            AnsiConsole.MarkupLine($"\n[cyan]Adding {dashboardChoice}...[/]");
            if (!await ComponentInstaller.InstallComponents(new[] { dashboardChoice! }, force: false, layoutSwitch))
                throw new InvalidOperationException($"{dashboardChoice} could not be installed.");
        }

        AnsiConsole.MarkupLine("\n[green]✅ ShellUI initialized successfully![/]");
        AnsiConsole.MarkupLine("\n[blue]Next steps:[/]");
        var step = 1;
        if (dashboardChoice != "none")
            AnsiConsole.MarkupLine($"  [dim]{step++}. Run the app:[/] dotnet watch");
        AnsiConsole.MarkupLine($"  [dim]{step++}. Pick a theme at[/] [link]https://tweakcn.com[/][dim], then:[/] shellui theme apply <theme-url>");
        AnsiConsole.MarkupLine($"  [dim]{step++}. Add components:[/] shellui add button");
        AnsiConsole.MarkupLine($"  [dim]{step}. Browse all:[/] shellui list");
    }

    private static async Task SetupTailwindNpmAsync()
    {
        if (!await IsNpmAvailableAsync())
        {
            throw new Exception("npm is not available. Please install Node.js and npm, or choose the 'standalone' Tailwind method.");
        }

        AnsiConsole.MarkupLine("[cyan]Installing Tailwind CSS packages...[/]");
        await RunNpmCommandAsync("install", "-D", $"tailwindcss@{TailwindConstants.NpmRange}", $"@tailwindcss/cli@{TailwindConstants.NpmRange}");
        AnsiConsole.MarkupLine($"[green]Installed:[/] tailwindcss v{TailwindConstants.Version}, @tailwindcss/cli");

        AnsiConsole.MarkupLine("[cyan]Creating CSS files...[/]");
        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        Directory.CreateDirectory(wwwrootPath);

        var inputCssPath = Path.Combine(wwwrootPath, "input.css");
        File.WriteAllText(inputCssPath, CssTemplates.InputCssNpm);
        AnsiConsole.MarkupLine($"[green]Created:[/] wwwroot/input.css");

        var appCssPath = Path.Combine(wwwrootPath, "app.css");
        File.WriteAllText(appCssPath, CssTemplates.AppCss);
        AnsiConsole.MarkupLine($"[green]Created:[/] wwwroot/app.css");

        var tailwindConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "tailwind.config.js");
        File.WriteAllText(tailwindConfigPath, CssTemplates.TailwindConfigJsNpm);
        AnsiConsole.MarkupLine($"[green]Created:[/] tailwind.config.js");
    }

    private static async Task<string?> SetupTailwindStandaloneAsync(Action<string> status)
    {
        var tailwindPath = await TailwindDownloader.EnsureTailwindCliAsync(Directory.GetCurrentDirectory(), status);

        AnsiConsole.MarkupLine("[cyan]Creating CSS files...[/]");
        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        Directory.CreateDirectory(wwwrootPath);

        var inputCssPath = Path.Combine(wwwrootPath, "input.css");
        File.WriteAllText(inputCssPath, CssTemplates.InputCss);
        AnsiConsole.MarkupLine($"[green]Created:[/] wwwroot/input.css");

        var appCssPath = Path.Combine(wwwrootPath, "app.css");
        File.WriteAllText(appCssPath, CssTemplates.AppCss);
        AnsiConsole.MarkupLine($"[green]Created:[/] wwwroot/app.css");

        var tailwindConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "tailwind.config.js");
        File.WriteAllText(tailwindConfigPath, CssTemplates.TailwindConfigJs);
        AnsiConsole.MarkupLine($"[green]Created:[/] tailwind.config.js");
        return tailwindPath;
    }

    private static async Task<bool> IsNpmAvailableAsync()
    {
        try
        {
            // cmd /c is the reliable way to run npm on Windows
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c npm --version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = System.Diagnostics.Process.Start(startInfo);
            if (process == null) return false;

            await process.WaitForExitAsync();
            var output = await process.StandardOutput.ReadToEndAsync();

            return process.ExitCode == 0 && !string.IsNullOrEmpty(output.Trim());
        }
        catch
        {
            return false;
        }
    }

    private static async Task RunNpmCommandAsync(params string[] args)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = $"/c npm {string.Join(" ", args)}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
        {
            throw new Exception("Failed to start npm process");
        }

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            throw new Exception($"npm command failed: {error}");
        }
    }

    private static async Task RunNpmTailwindBuildAsync()
    {
        var inputCssPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "input.css");
        var outputCssPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app.css");

        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = $"/c npx @tailwindcss/cli -i \"{inputCssPath}\" -o \"{outputCssPath}\" --minify",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
        {
            throw new Exception("Failed to start npx process");
        }

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            throw new Exception($"npx @tailwindcss/cli failed: {error}");
        }
    }

    private static async Task RunTailwindBuildAsync(string tailwindPath, string inputPath, string outputPath)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = tailwindPath,
            Arguments = $"-i \"{inputPath}\" -o \"{outputPath}\" --minify",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = System.Diagnostics.Process.Start(startInfo);
        if (process == null)
        {
            throw new Exception("Failed to start Tailwind CSS process");
        }

        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
        {
            var error = await process.StandardError.ReadToEndAsync();
            throw new Exception($"Tailwind CSS build failed: {error}");
        }
    }

    // Build/ShellUI.targets exactly as earlier CLIs wrote it for standalone Tailwind (LF-normalized SHA-256): 0.0.1–0.3.4, then 0.4.0-alpha.1.
    private static readonly string[] PreviousStandaloneTargets =
    {
        "eace414bc927d6274976e888cd1de742efd82ee3142f1cabaea3b1f3e188455e",
        "f42811cbd3bdad588c3389de9a0f706b7ac59331848a027b99c30a5765aea5f9",
    };

    // Brings a standalone project set up by an earlier CLI onto the shared Tailwind CLI; returns what changed.
    internal static List<string> RefreshTailwindSetup(string projectRoot)
    {
        var notes = new List<string>();
        var path = Path.Combine(projectRoot, "Build", "ShellUI.targets");
        if (!File.Exists(path)) return notes;

        var current = GetTargetsFileContent("standalone");
        var hash = ComponentInstaller.ContentHash(File.ReadAllText(path));
        if (PreviousStandaloneTargets.Contains(hash))
        {
            File.WriteAllText(path, current);
            notes.Add("[green]Updated:[/] Build/ShellUI.targets [dim](uses one shared Tailwind CLI per machine and retries its download)[/]");
        }
        else if (hash != ComponentInstaller.ContentHash(current))
        {
            if (File.ReadAllText(path).Contains("TailwindExecutable", StringComparison.Ordinal))
                notes.Add("[yellow]Note:[/] Build/ShellUI.targets has local changes, so it was not updated. Compare it with a new [bold]shellui init[/] to pick up the shared Tailwind CLI.");
            return notes;
        }

        TailwindDownloader.IgnoreShellUiFolder(projectRoot);
        if (TailwindDownloader.RemoveDuplicateProjectCopy(projectRoot))
            notes.Add($"[green]Removed:[/] the project's copy of the Tailwind CLI [dim](builds use the shared one in {Markup.Escape(TailwindDownloader.CacheDirectory())})[/]");
        return notes;
    }

    internal static string GetTargetsFileContent(string method)
    {
        if (method == "npm")
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project>
  <PropertyGroup>
    <NpmExecutable>npx</NpmExecutable>
    <TailwindInputCss Condition=""'$(TailwindInputCss)' == ''"">$(MSBuildProjectDirectory)\wwwroot\input.css</TailwindInputCss>
    <TailwindOutputCss Condition=""'$(TailwindOutputCss)' == ''"">$(MSBuildProjectDirectory)\wwwroot\app.css</TailwindOutputCss>
    <TailwindMinify Condition=""'$(Configuration)' == 'Release'"">--minify</TailwindMinify>
    <TailwindMinify Condition=""'$(Configuration)' != 'Release'""></TailwindMinify>
  </PropertyGroup>

  <Target Name=""BuildTailwindCSS"" BeforeTargets=""BeforeBuild"" Condition=""Exists('$(TailwindInputCss)')"">
    <Message Importance=""high"" Text=""Building Tailwind CSS with npm..."" />
    <Exec Command=""$(NpmExecutable) @tailwindcss/cli -i &quot;$(TailwindInputCss)&quot; -o &quot;$(TailwindOutputCss)&quot; $(TailwindMinify)"" />
    <Message Importance=""high"" Text=""Tailwind CSS built successfully!"" />
  </Target>

  <Target Name=""CleanTailwindCSS"" AfterTargets=""Clean"" Condition=""Exists('$(TailwindOutputCss)')"">
    <Message Importance=""high"" Text=""Cleaning Tailwind CSS output..."" />
    <Delete Files=""$(TailwindOutputCss)"" />
  </Target>
</Project>";
        }
        else
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project>
  <PropertyGroup>
    <ShellUIBinPath Condition=""'$(ShellUIBinPath)' == ''"">$(MSBuildProjectDirectory)\.shellui\bin</ShellUIBinPath>
    <TailwindInputCss Condition=""'$(TailwindInputCss)' == ''"">$(MSBuildProjectDirectory)\wwwroot\input.css</TailwindInputCss>
    <TailwindOutputCss Condition=""'$(TailwindOutputCss)' == ''"">$(MSBuildProjectDirectory)\wwwroot\app.css</TailwindOutputCss>
    <TailwindMinify Condition=""'$(Configuration)' == 'Release'"">--minify</TailwindMinify>
    <TailwindMinify Condition=""'$(Configuration)' != 'Release'""></TailwindMinify>
    <TailwindTag>__TAILWIND_TAG__</TailwindTag>
    <TailwindVersion>__TAILWIND_VERSION__</TailwindVersion>
    <TailwindArch>$([System.Runtime.InteropServices.RuntimeInformation]::OSArchitecture.ToString().ToLowerInvariant())</TailwindArch>
    <TailwindPlatform Condition=""$([MSBuild]::IsOSPlatform('Windows'))"">windows-$(TailwindArch).exe</TailwindPlatform>
    <TailwindPlatform Condition=""$([MSBuild]::IsOSPlatform('Linux'))"">linux-$(TailwindArch)</TailwindPlatform>
    <TailwindPlatform Condition=""$([MSBuild]::IsOSPlatform('OSX'))"">macos-$(TailwindArch)</TailwindPlatform>
    <!-- One copy per machine, shared with the ShellUI CLI, so projects hold no binary; SHELLUI_CACHE_DIR moves it. -->
    <ShellUICacheDir Condition=""'$(ShellUICacheDir)' == ''"">$(SHELLUI_CACHE_DIR)</ShellUICacheDir>
    <ShellUICacheDir Condition=""'$(ShellUICacheDir)' == ''"">$([System.IO.Path]::Combine($([System.Environment]::GetFolderPath(SpecialFolder.UserProfile)), '.shellui', 'bin'))</ShellUICacheDir>
    <TailwindCached>$([System.IO.Path]::Combine('$(ShellUICacheDir)', 'tailwindcss-$(TailwindVersion)-$(TailwindPlatform)'))</TailwindCached>
    <!-- A copy in the project's .shellui/bin wins over the shared one. -->
    <TailwindExecutable Condition=""'$(TailwindExecutable)' == '' AND '$(OS)' == 'Windows_NT' AND Exists('$(ShellUIBinPath)\tailwindcss.exe')"">$(ShellUIBinPath)\tailwindcss.exe</TailwindExecutable>
    <TailwindExecutable Condition=""'$(TailwindExecutable)' == '' AND '$(OS)' != 'Windows_NT' AND Exists('$(ShellUIBinPath)/tailwindcss')"">$(ShellUIBinPath)/tailwindcss</TailwindExecutable>
    <TailwindExecutable Condition=""'$(TailwindExecutable)' == ''"">$(TailwindCached)</TailwindExecutable>
  </PropertyGroup>

  <!-- Downloads to .partial and renames only on success, so an interrupted download never looks like a working CLI. -->
  <Target Name=""DownloadTailwindCLI"" BeforeTargets=""BeforeBuild"" Condition=""!Exists('$(TailwindExecutable)') AND Exists('$(TailwindInputCss)') AND '$(TailwindPlatform)' != ''"">
    <Message Importance=""high"" Text=""Downloading the Tailwind CLI $(TailwindTag) to $(ShellUICacheDir)..."" />
    <DownloadFile SourceUrl=""https://github.com/tailwindlabs/tailwindcss/releases/download/$(TailwindTag)/tailwindcss-$(TailwindPlatform)"" DestinationFolder=""$(ShellUICacheDir)"" DestinationFileName=""tailwindcss-$(TailwindVersion)-$(TailwindPlatform).partial"" Retries=""3"" RetryDelayMilliseconds=""5000"" Timeout=""1800000"" ContinueOnError=""true"" />
    <Move SourceFiles=""$(TailwindCached).partial"" DestinationFiles=""$(TailwindCached)"" Condition=""'$(MSBuildLastTaskResult)' == 'true' AND Exists('$(TailwindCached).partial')"" />
    <Delete Files=""$(TailwindCached).partial"" ContinueOnError=""true"" />
    <Exec Command=""chmod +x &quot;$(TailwindCached)&quot;"" Condition=""'$(OS)' != 'Windows_NT' AND Exists('$(TailwindCached)')"" />
  </Target>

  <Target Name=""WarnMissingTailwindCLI"" BeforeTargets=""BeforeBuild"" Condition=""!Exists('$(TailwindExecutable)') AND Exists('$(TailwindInputCss)')"">
    <Warning Text=""ShellUI: the Tailwind CLI could not be downloaded, so wwwroot/app.css was not rebuilt. Save https://github.com/tailwindlabs/tailwindcss/releases/download/$(TailwindTag)/tailwindcss-$(TailwindPlatform) as $(TailwindCached), or build again to retry."" />
  </Target>

  <Target Name=""BuildTailwindCSS"" BeforeTargets=""BeforeBuild"" Condition=""Exists('$(TailwindExecutable)') AND Exists('$(TailwindInputCss)')"">
    <Message Importance=""high"" Text=""Building Tailwind CSS..."" />
    <Exec Command=""&quot;$(TailwindExecutable)&quot; -i &quot;$(TailwindInputCss)&quot; -o &quot;$(TailwindOutputCss)&quot; $(TailwindMinify)"" />
    <Message Importance=""high"" Text=""Tailwind CSS built successfully!"" />
  </Target>

  <Target Name=""CleanTailwindCSS"" AfterTargets=""Clean"" Condition=""Exists('$(TailwindOutputCss)')"">
    <Message Importance=""high"" Text=""Cleaning Tailwind CSS output..."" />
    <Delete Files=""$(TailwindOutputCss)"" />
  </Target>
</Project>".Replace("__TAILWIND_TAG__", TailwindConstants.GitHubTag).Replace("__TAILWIND_VERSION__", TailwindConstants.Version);
        }
    }

    private static async Task UpdateProjectFileAsync(string projectFilePath, string targetsPath)
    {
        var content = await File.ReadAllTextAsync(projectFilePath);
        var targetsImport = $"  <Import Project=\"Build\\ShellUI.targets\" />";

        if (content.Contains(targetsImport) || content.Contains("ShellUI.targets"))
        {
            return;
        }

        var closingTag = "</Project>";
        var insertIndex = content.LastIndexOf(closingTag);
        
        if (insertIndex > 0)
        {
            content = content.Insert(insertIndex, targetsImport + Environment.NewLine + Environment.NewLine);
            await File.WriteAllTextAsync(projectFilePath, content);
        }
    }

    private static async Task BootstrapHostAsync(ProjectInfo projectInfo)
    {
        var cwd = Directory.GetCurrentDirectory();

        var appRazor = Path.Combine(cwd, "Components", "App.razor");
        if (File.Exists(appRazor))
        {
            var original = await File.ReadAllTextAsync(appRazor);
            var patched = RewriteAppRazor(original, DashboardSetup.IsIdentityApp(cwd));
            if (patched != original)
            {
                await File.WriteAllTextAsync(appRazor, patched);
                AnsiConsole.MarkupLine("[green]Patched:[/] Components/App.razor");
            }
            return;
        }

        // WASM has no render mode to patch; only the theme bootstrap and shellui.js are added.
        var indexHtml = Path.Combine(cwd, "wwwroot", "index.html");
        if (File.Exists(indexHtml))
        {
            var original = await File.ReadAllTextAsync(indexHtml);
            var patched = RewriteWasmIndexHtml(original);
            if (patched != original)
            {
                await File.WriteAllTextAsync(indexHtml, patched);
                AnsiConsole.MarkupLine("[green]Patched:[/] wwwroot/index.html");
            }
            return;
        }

        if (RazorClassLibraryAssetBase(projectInfo.ProjectPath) is { } assetBase)
        {
            AnsiConsole.MarkupLine("[cyan]This is a Razor class library.[/] Each app that uses it needs these steps:");
            foreach (var step in RazorClassLibraryHostSteps(assetBase))
                AnsiConsole.MarkupLine($"  {Markup.Escape(step)}");
            return;
        }

        AnsiConsole.MarkupLine("[yellow]No Components/App.razor or wwwroot/index.html found — skipped host bootstrap.[/]");
    }

    internal const string RazorClassLibraryGuide = "https://github.com/shellui-dev/shellui/blob/main/docs/razor-class-library.md";

    // The URL prefix a library's wwwroot is served under in the apps that reference it, or null when the
    // project isn't a Razor class library (no host page of its own and no Program.cs).
    internal static string? RazorClassLibraryAssetBase(string csprojPath)
    {
        if (!File.Exists(csprojPath)) return null;
        var doc = XDocument.Load(csprojPath);
        if (!string.Equals((string?)doc.Root?.Attribute("Sdk"), "Microsoft.NET.Sdk.Razor", StringComparison.OrdinalIgnoreCase)) return null;
        if (File.Exists(Path.Combine(Path.GetDirectoryName(csprojPath)!, "Program.cs"))) return null;

        string? Property(string name) => doc.Descendants(name).FirstOrDefault()?.Value.Trim() is { Length: > 0 } value ? value : null;
        if (Property("StaticWebAssetBasePath") is { } basePath) return basePath.Trim('/');
        return "_content/" + (Property("PackageId") ?? Property("AssemblyName") ?? Path.GetFileNameWithoutExtension(csprojPath));
    }

    internal static IReadOnlyList<string> RazorClassLibraryHostSteps(string assetBase) => new[]
    {
        $"1. In the app's host page (Components/App.razor, or wwwroot/index.html in a MAUI Blazor Hybrid or WebAssembly app), link <link rel=\"stylesheet\" href=\"{assetBase}/app.css\" /> and load <script src=\"{assetBase}/shellui.js\"></script> before the Blazor script. Don't copy app.css into the app.",
        "2. Add the ShellUI theme script to the host page's <head>, so dark mode applies before the first paint.",
        "3. Tailwind only scans this library: for classes in the app's own markup, add @source \"<path to the app>\"; to this library's wwwroot/input.css.",
        "4. Apps that keep state in browser storage render without prerendering: new InteractiveServerRenderMode(prerender: false).",
        $"Guide: {RazorClassLibraryGuide}",
    };

    // Idempotent. Tags that already have attributes are left for the user.
    internal const string IdentityRenderMode =
@"@code {
    [CascadingParameter]
    private HttpContext HttpContext { get; set; } = default!;

    // The Identity pages under /Account set cookies, so they need a static render with a real HTTP request.
    private IComponentRenderMode? PageRenderMode =>
        HttpContext.Request.Path.StartsWithSegments(""/Account"") ? null : InteractiveServer;
}
";

    internal static string RewriteAppRazor(string content, bool identity = false)
    {
        // init deletes the template's local Bootstrap copy; CDN links are left alone.
        content = Regex.Replace(content, @"^[ \t]*<link\b(?![^\r\n]*://)[^\r\n]*bootstrap[^\r\n]*\.css[^\r\n]*\r?\n", "", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        var renderMode = identity ? "PageRenderMode" : "InteractiveServer";
        var beforeRenderMode = content;
        content = Regex.Replace(content, @"<HeadOutlet\s*/>", $@"<HeadOutlet @rendermode=""{renderMode}"" />");
        content = Regex.Replace(content, @"<Routes\s*/>", $@"<Routes @rendermode=""{renderMode}"" />");
        if (identity && content != beforeRenderMode && !content.Contains("PageRenderMode =>", StringComparison.Ordinal))
        {
            var newline = content.Contains("\r\n") ? "\r\n" : "\n";
            content = content.TrimEnd() + newline + newline + IdentityRenderMode.Replace("\r\n", "\n").Replace("\n", newline);
        }

        // Sets `dark` before first paint to avoid a light flash.
        if (!content.Contains("ShellUI theme bootstrap"))
        {
            const string themeScript =
@"    <script>
        // ShellUI theme bootstrap — runs before Blazor mounts to avoid a light-flash on dark pages.
        if (!localStorage.getItem('theme')) { localStorage.setItem('theme', 'light'); }
        if (localStorage.getItem('theme') === 'dark') {
            document.documentElement.classList.add('dark');
        }
    </script>
";
            content = Regex.Replace(content, @"</head>", themeScript + "</head>", RegexOptions.IgnoreCase);
        }

        // shellui.js must load before blazor.web.js; matches both @Assets[...] and the bare form.
        if (!content.Contains("shellui.js"))
        {
            content = Regex.Replace(
                content,
                @"(<script\s+src=[^>]*_framework/blazor\.web\.js[^>]*>)",
                "<script src=\"shellui.js\"></script>\n    $1");
        }

        return content;
    }

    internal static string RewriteWasmIndexHtml(string content)
    {
        if (!content.Contains("ShellUI theme bootstrap"))
        {
            const string themeScript =
@"    <script>
        // ShellUI theme bootstrap — runs before Blazor mounts to avoid a light-flash on dark pages.
        if (!localStorage.getItem('theme')) { localStorage.setItem('theme', 'light'); }
        if (localStorage.getItem('theme') === 'dark') {
            document.documentElement.classList.add('dark');
        }
    </script>
";
            content = Regex.Replace(content, @"</head>", themeScript + "</head>", RegexOptions.IgnoreCase);
        }

        if (!content.Contains("shellui.js"))
        {
            content = Regex.Replace(
                content,
                @"(<script\s+src=[^>]*_framework/blazor\.webassembly\.js[^>]*>)",
                "<script src=\"shellui.js\"></script>\n    $1");
        }

        return content;
    }

    internal static string InjectStylesheetLink(string content, string href)
    {
        var linkTag = $"<link href=\"{href}\" rel=\"stylesheet\" />";
        if (content.Contains($"href=\"{href}\""))
        {
            return content;
        }
        return Regex.Replace(content, @"</head>", $"    {linkTag}\n</head>", RegexOptions.IgnoreCase);
    }

    internal static async Task InjectStylesheetIntoHostAsync(string href)
    {
        var cwd = Directory.GetCurrentDirectory();

        var appRazor = Path.Combine(cwd, "Components", "App.razor");
        if (File.Exists(appRazor))
        {
            var original = await File.ReadAllTextAsync(appRazor);
            var patched = InjectStylesheetLink(original, href);
            if (patched != original)
            {
                await File.WriteAllTextAsync(appRazor, patched);
                AnsiConsole.MarkupLine($"[green]Linked:[/] Components/App.razor → {href}");
            }
            return;
        }

        var indexHtml = Path.Combine(cwd, "wwwroot", "index.html");
        if (File.Exists(indexHtml))
        {
            var original = await File.ReadAllTextAsync(indexHtml);
            var patched = InjectStylesheetLink(original, href);
            if (patched != original)
            {
                await File.WriteAllTextAsync(indexHtml, patched);
                AnsiConsole.MarkupLine($"[green]Linked:[/] wwwroot/index.html → {href}");
            }
        }
    }

    private static void RemoveBootstrapFiles()
    {
        try
        {
            var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (!Directory.Exists(wwwrootPath)) return;

            AnsiConsole.MarkupLine("[cyan]Checking for Bootstrap files to clean up...[/]");
            var deletedCount = 0;

            foreach (var folder in new[] { Path.Combine("lib", "bootstrap"), "bootstrap" })
            {
                var bootstrapDir = Path.Combine(wwwrootPath, folder);
                if (!Directory.Exists(bootstrapDir)) continue;
                Directory.Delete(bootstrapDir, true);
                AnsiConsole.MarkupLine($"[dim]Deleted:[/] wwwroot/{folder.Replace('\\', '/')} folder");
                deletedCount++;
            }

            var cssPath = Path.Combine(wwwrootPath, "css");
            if (Directory.Exists(cssPath))
            {
                var bootstrapFiles = Directory.GetFiles(cssPath, "bootstrap*.*");
                foreach (var file in bootstrapFiles)
                {
                    File.Delete(file);
                    AnsiConsole.MarkupLine($"[dim]Deleted:[/] css/{Path.GetFileName(file)}");
                    deletedCount++;
                }
            }
            
            if (deletedCount > 0)
            {
                AnsiConsole.MarkupLine($"[green]Removed {deletedCount} Bootstrap items.[/]");
            }
        }
        catch (Exception ex)
        {
             AnsiConsole.MarkupLine($"[yellow]Warning: Could not remove some Bootstrap files: {ex.Message}[/]");
        }
    }
}

