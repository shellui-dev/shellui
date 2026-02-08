using ShellUI.Core.Models;
using ShellUI.Templates;
using System.Text.Json;
using Spectre.Console;

namespace ShellUI.CLI.Services;

public class InitService
{
    public static async Task InitializeAsync(string style, bool force, string tailwindMethod = "standalone", bool nonInteractive = false)
    {
        var configPath = Path.Combine(Directory.GetCurrentDirectory(), "shellui.json");

        if (File.Exists(configPath) && !force)
        {
            AnsiConsole.MarkupLine("[yellow]ShellUI is already initialized in this project.[/]");
            AnsiConsole.MarkupLine("[dim]Use --force to reinitialize[/]");
            return;
        }

        // Step 1: Detect project
        ProjectInfo projectInfo = null!;

        try
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(Style.Parse("green"))
                .StartAsync("Initializing ShellUI...", async ctx =>
                {
                    ctx.Status("Detecting project type...");
                    await Task.Delay(300); // Brief delay for UX
                    projectInfo = ProjectDetector.DetectProject();
                    AnsiConsole.MarkupLine($"[green]✅ Detected:[/] {projectInfo.ProjectType}");
                    AnsiConsole.MarkupLine($"[dim]Project: {projectInfo.ProjectName}[/]");
                    AnsiConsole.MarkupLine($"[dim]Namespace: {projectInfo.RootNamespace}[/]");

                    // Clean up bootstrap files
                    ctx.Status("Cleaning up Bootstrap files...");
                    RemoveBootstrapFiles();
                });
        }
        catch
        {
            // Fallback if status display fails
            projectInfo = ProjectDetector.DetectProject();
            AnsiConsole.MarkupLine($"[green]✅ Detected:[/] {projectInfo.ProjectType}");
            AnsiConsole.MarkupLine($"[dim]Project: {projectInfo.ProjectName}[/]");
            AnsiConsole.MarkupLine($"[dim]Namespace: {projectInfo.RootNamespace}[/]");

            // Clean up bootstrap files
            RemoveBootstrapFiles();
        }

        // Step 2: Determine Tailwind method preference
        AnsiConsole.MarkupLine("[cyan]Setting up Tailwind CSS...[/]");
        string method;

        if (nonInteractive)
        {
            method = tailwindMethod;
            AnsiConsole.MarkupLine($"[green]✅ Selected:[/] {method} (non-interactive mode)");
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

        // Check npm availability if selected
        if (method == "npm" && !await IsNpmAvailableAsync())
        {
            AnsiConsole.MarkupLine("[red]Error: npm is not available. Please install Node.js and npm, or choose the 'standalone' Tailwind method.[/]");
            return;
        }

        await AnsiConsole.Status()
            .StartAsync("Installing ShellUI components...", async ctx => 
            {
                // Step 3: Create Components/UI folder
                ctx.Status("Creating component folders...");
                var componentsPath = Path.Combine(Directory.GetCurrentDirectory(), "Components", "UI");
                Directory.CreateDirectory(componentsPath);
                AnsiConsole.MarkupLine($"[green]Created:[/] Components/UI/");

                // Step 3.5: Install Shell utilities
                ctx.Status("Installing Shell utilities...");
                await ComponentInstaller.InstallComponentForInitAsync("shell", projectInfo);

                // Step 4: Create shellui.json
                ctx.Status("Creating configuration...");
                var config = new ShellUIConfig
                {
                    Style = style,
                    ComponentsPath = "Components/UI",
                    ProjectType = projectInfo.ProjectType,
                    Tailwind = new TailwindConfig
                    {
                        Enabled = true,
                        Version = "4.1.18",
                        Method = method,
                        CssPath = "wwwroot/app.css"
                    }
                };

                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(configPath, json);
                AnsiConsole.MarkupLine($"[green]✅ Created:[/] shellui.json");

                // Step 5: Create _Imports.razor if it doesn't exist
                ctx.Status("Setting up imports...");
                var importsPath = Path.Combine(Directory.GetCurrentDirectory(), "Components", "_Imports.razor");
                if (File.Exists(importsPath))
                {
                    var importsContent = File.ReadAllText(importsPath);
                    var usingStatement = $"@using {projectInfo.RootNamespace}.Components.UI";

                    if (!importsContent.Contains(usingStatement))
                    {
                        File.AppendAllText(importsPath, $"\n{usingStatement}\n");
                        AnsiConsole.MarkupLine($"[green]Updated:[/] Components/_Imports.razor");
                    }
                }

                // Step 6: Set up Tailwind CSS based on method
                ctx.Status("Setting up Tailwind CSS...");
                if (method == "npm")
                {
                    await SetupTailwindNpmAsync();
                }
                else
                {
                    await SetupTailwindStandaloneAsync();
                }

                // Step 7: Create MSBuild targets file
                ctx.Status("Setting up MSBuild integration...");
                var buildPath = Path.Combine(Directory.GetCurrentDirectory(), "Build");
                Directory.CreateDirectory(buildPath);

                var targetsPath = Path.Combine(buildPath, "ShellUI.targets");
                var targetsContent = GetTargetsFileContent(method);
                File.WriteAllText(targetsPath, targetsContent);
                AnsiConsole.MarkupLine($"[green]Created:[/] Build/ShellUI.targets");

                // Update .csproj to import targets
                await UpdateProjectFileAsync(projectInfo.ProjectPath, targetsPath);
                AnsiConsole.MarkupLine($"[green]Updated:[/] {Path.GetFileName(projectInfo.ProjectPath)}");

                // Step 8: Run initial Tailwind build
                ctx.Status("Building Tailwind CSS...");
                if (method == "npm")
                {
                    await RunNpmTailwindBuildAsync();
                }
                else
                {
                    var tailwindPath = await TailwindDownloader.EnsureTailwindCliAsync(Directory.GetCurrentDirectory());
                    var inputCssPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "input.css");
                    var appCssPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "app.css");
                    await RunTailwindBuildAsync(tailwindPath, inputCssPath, appCssPath);
                }
                AnsiConsole.MarkupLine($"[green]Built:[/] Tailwind CSS");
            });

        AnsiConsole.MarkupLine("\n[green]✅ ShellUI initialized successfully![/]");
        AnsiConsole.MarkupLine("\n[blue]Next steps:[/]");
        AnsiConsole.MarkupLine("  [dim]1. Add components:[/] dotnet shellui add button");
        AnsiConsole.MarkupLine("  [dim]2. Browse all:[/] dotnet shellui list");
    }

    private static async Task SetupTailwindNpmAsync()
    {
        // Check if npm is available
        if (!await IsNpmAvailableAsync())
        {
            throw new Exception("npm is not available. Please install Node.js and npm, or choose the 'standalone' Tailwind method.");
        }

        // Install Tailwind CSS packages (v4 with @tailwindcss/cli)
        AnsiConsole.MarkupLine("[cyan]Installing Tailwind CSS packages...[/]");
        await RunNpmCommandAsync("install", "-D", "tailwindcss@^4.1.17", "@tailwindcss/cli@^4.1.17");
        AnsiConsole.MarkupLine("[green]Installed:[/] tailwindcss v4.1.17, @tailwindcss/cli");

        // Create CSS files
        AnsiConsole.MarkupLine("[cyan]Creating CSS files...[/]");
        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        Directory.CreateDirectory(wwwrootPath);

        // Create input.css with npm-compatible imports
        var inputCssPath = Path.Combine(wwwrootPath, "input.css");
        File.WriteAllText(inputCssPath, CssTemplates.InputCssNpm);
        AnsiConsole.MarkupLine($"[green]Created:[/] wwwroot/input.css");

        // Create placeholder app.css
        var appCssPath = Path.Combine(wwwrootPath, "app.css");
        File.WriteAllText(appCssPath, CssTemplates.AppCss);
        AnsiConsole.MarkupLine($"[green]Created:[/] wwwroot/app.css");

        // Create tailwind.config.js for npm
        var tailwindConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "tailwind.config.js");
        File.WriteAllText(tailwindConfigPath, CssTemplates.TailwindConfigJsNpm);
        AnsiConsole.MarkupLine($"[green]Created:[/] tailwind.config.js");
    }

    private static async Task SetupTailwindStandaloneAsync()
    {
        // Download Tailwind CLI
        AnsiConsole.MarkupLine("[cyan]Downloading Tailwind CSS standalone CLI...[/]");
        var tailwindPath = await TailwindDownloader.EnsureTailwindCliAsync(Directory.GetCurrentDirectory());

        // Create CSS files
        AnsiConsole.MarkupLine("[cyan]Creating CSS files...[/]");
        var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        Directory.CreateDirectory(wwwrootPath);

        // Create input.css with design tokens
        var inputCssPath = Path.Combine(wwwrootPath, "input.css");
        File.WriteAllText(inputCssPath, CssTemplates.InputCss);
        AnsiConsole.MarkupLine($"[green]Created:[/] wwwroot/input.css");

        // Create placeholder app.css
        var appCssPath = Path.Combine(wwwrootPath, "app.css");
        File.WriteAllText(appCssPath, CssTemplates.AppCss);
        AnsiConsole.MarkupLine($"[green]Created:[/] wwwroot/app.css");

        // Create tailwind.config.js
        var tailwindConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "tailwind.config.js");
        File.WriteAllText(tailwindConfigPath, CssTemplates.TailwindConfigJs);
        AnsiConsole.MarkupLine($"[green]Created:[/] tailwind.config.js");
    }

    private static async Task<bool> IsNpmAvailableAsync()
    {
        try
        {
            // Try cmd /c npm --version (most reliable on Windows)
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

        // Use npx to run tailwindcss
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = $"/c npx tailwindcss -i \"{inputCssPath}\" -o \"{outputCssPath}\" --minify",
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
            throw new Exception($"npx tailwindcss failed: {error}");
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

    private static string GetTargetsFileContent(string method)
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
    <TailwindExecutable Condition=""'$(OS)' == 'Windows_NT'"">$(ShellUIBinPath)\tailwindcss.exe</TailwindExecutable>
    <TailwindExecutable Condition=""'$(OS)' != 'Windows_NT'"">$(ShellUIBinPath)/tailwindcss</TailwindExecutable>
    <TailwindInputCss Condition=""'$(TailwindInputCss)' == ''"">$(MSBuildProjectDirectory)\wwwroot\input.css</TailwindInputCss>
    <TailwindOutputCss Condition=""'$(TailwindOutputCss)' == ''"">$(MSBuildProjectDirectory)\wwwroot\app.css</TailwindOutputCss>
    <TailwindMinify Condition=""'$(Configuration)' == 'Release'"">--minify</TailwindMinify>
    <TailwindMinify Condition=""'$(Configuration)' != 'Release'""></TailwindMinify>
  </PropertyGroup>

  <Target Name=""BuildTailwindCSS"" BeforeTargets=""BeforeBuild"" Condition=""Exists('$(TailwindExecutable)') AND Exists('$(TailwindInputCss)')"">
    <Message Importance=""high"" Text=""Building Tailwind CSS..."" />
    <Exec Command=""&quot;$(TailwindExecutable)&quot; -i &quot;$(TailwindInputCss)&quot; -o &quot;$(TailwindOutputCss)&quot; $(TailwindMinify)"" />
    <Message Importance=""high"" Text=""Tailwind CSS built successfully!"" />
  </Target>

  <Target Name=""CleanTailwindCSS"" AfterTargets=""Clean"" Condition=""Exists('$(TailwindOutputCss)')"">
    <Message Importance=""high"" Text=""Cleaning Tailwind CSS output..."" />
    <Delete Files=""$(TailwindOutputCss)"" />
  </Target>
</Project>";
        }
    }

    private static async Task UpdateProjectFileAsync(string projectFilePath, string targetsPath)
    {
        var content = await File.ReadAllTextAsync(projectFilePath);
        var targetsImport = $"  <Import Project=\"Build\\ShellUI.targets\" />";

        // Check if import already exists
        if (content.Contains(targetsImport) || content.Contains("ShellUI.targets"))
        {
            return;
        }

        // Insert before closing </Project> tag
        var closingTag = "</Project>";
        var insertIndex = content.LastIndexOf(closingTag);
        
        if (insertIndex > 0)
        {
            content = content.Insert(insertIndex, targetsImport + Environment.NewLine + Environment.NewLine);
            await File.WriteAllTextAsync(projectFilePath, content);
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

            // 1. Delete wwwroot/lib/bootstrap
            var libBootstrap = Path.Combine(wwwrootPath, "lib", "bootstrap");
            if (Directory.Exists(libBootstrap))
            {
                Directory.Delete(libBootstrap, true);
                AnsiConsole.MarkupLine($"[dim]Deleted:[/] wwwroot/lib/bootstrap folder");
                deletedCount++;
            }

            // 2. Delete bootstrap css files in wwwroot/css
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

