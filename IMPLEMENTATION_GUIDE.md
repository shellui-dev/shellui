# ShellUI Implementation Guide

**Step-by-step guide to build ShellUI from scratch**

## Current Status

- ✅ Planning Complete (15 comprehensive docs)
- 🚀 **NOW:** Start building Milestone 1
- 📍 **You are here:** Creating project structure

## Step 1: Create Project Structure

### Commands to Run

```bash
# Navigate to your repo root
cd C:\devcode\shewart\shelltech\shellui\shell-ui

# Create src directory
mkdir src

# Create CLI tool project (this becomes 'dotnet shellui' command)
dotnet new tool -n ShellUI.CLI -o src/ShellUI.CLI

# Create Razor Class Library for NuGet package
dotnet new razorclasslib -n ShellUI.Components -o src/ShellUI.Components

# Create shared core library
dotnet new classlib -n ShellUI.Core -o src/ShellUI.Core

# Create templates library (for component templates)
dotnet new classlib -n ShellUI.Templates -o src/ShellUI.Templates

# Create solution file
dotnet new sln -n ShellUI

# Add all projects to solution
dotnet sln ShellUI.sln add src/ShellUI.CLI/ShellUI.CLI.csproj
dotnet sln ShellUI.sln add src/ShellUI.Components/ShellUI.Components.csproj
dotnet sln ShellUI.sln add src/ShellUI.Core/ShellUI.Core.csproj
dotnet sln ShellUI.sln add src/ShellUI.Templates/ShellUI.Templates.csproj

# Add project references
cd src/ShellUI.CLI
dotnet add reference ../ShellUI.Core/ShellUI.Core.csproj
dotnet add reference ../ShellUI.Templates/ShellUI.Templates.csproj

cd ../ShellUI.Components
dotnet add reference ../ShellUI.Core/ShellUI.Core.csproj

cd ../..
```

### Add Required NuGet Packages

```bash
# CLI tool dependencies
cd src/ShellUI.CLI
dotnet add package System.CommandLine --version 2.0.0-beta4.22272.1
dotnet add package Spectre.Console --version 0.49.1

# Core dependencies
cd ../ShellUI.Core
dotnet add package System.Text.Json --version 8.0.0

# Components dependencies
cd ../ShellUI.Components
dotnet add package Microsoft.AspNetCore.Components.Web --version 8.0.0

cd ../..
```

### Project Structure After Step 1

```
shell-ui/
├── ShellUI.sln                   # ← NEW
├── src/                          # ← NEW
│   ├── ShellUI.CLI/              # ← NEW (dotnet tool)
│   │   ├── Commands/
│   │   ├── Services/
│   │   ├── Models/
│   │   ├── Program.cs
│   │   └── ShellUI.CLI.csproj
│   ├── ShellUI.Components/       # ← NEW (NuGet package)
│   │   ├── Components/
│   │   ├── wwwroot/
│   │   └── ShellUI.Components.csproj
│   ├── ShellUI.Core/             # ← NEW (shared logic)
│   │   ├── Models/
│   │   ├── Services/
│   │   └── ShellUI.Core.csproj
│   └── ShellUI.Templates/        # ← NEW (component templates)
│       ├── Templates/
│       └── ShellUI.Templates.csproj
├── registry/                     # Will create later
├── NET8/                         # ← EXISTING (examples)
├── NET9/                         # ← EXISTING (examples)
└── docs/                         # ← All our planning docs
```

## Step 2: Configure CLI Tool

### Update ShellUI.CLI.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    
    <!-- CLI Tool Configuration -->
    <PackAsTool>true</PackAsTool>
    <ToolCommandName>shellui</ToolCommandName>
    <PackageId>ShellUI.CLI</PackageId>
    <Version>0.1.0-alpha</Version>
    <Authors>ShellUI Contributors</Authors>
    <Description>CLI tool for ShellUI - Blazor component library</Description>
    <PackageTags>blazor;components;cli;shadcn;tailwind</PackageTags>
    <PackageProjectUrl>https://github.com/yourorg/shellui</PackageProjectUrl>
    <RepositoryUrl>https://github.com/yourorg/shellui</RepositoryUrl>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="System.CommandLine" Version="2.0.0-beta4.22272.1" />
    <PackageReference Include="Spectre.Console" Version="0.49.1" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\ShellUI.Core\ShellUI.Core.csproj" />
    <ProjectReference Include="..\ShellUI.Templates\ShellUI.Templates.csproj" />
  </ItemGroup>

</Project>
```

### Create Basic Program.cs

```csharp
// src/ShellUI.CLI/Program.cs
using System.CommandLine;
using Spectre.Console;

namespace ShellUI.CLI;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand("ShellUI - CLI for Blazor component library");

        // Add commands
        rootCommand.AddCommand(CreateInitCommand());
        rootCommand.AddCommand(CreateAddCommand());
        rootCommand.AddCommand(CreateListCommand());

        return await rootCommand.InvokeAsync(args);
    }

    static Command CreateInitCommand()
    {
        var command = new Command("init", "Initialize ShellUI in your project");
        
        command.SetHandler(() =>
        {
            AnsiConsole.MarkupLine("[green]Initializing ShellUI...[/]");
            AnsiConsole.MarkupLine("[yellow]Coming soon![/]");
        });

        return command;
    }

    static Command CreateAddCommand()
    {
        var command = new Command("add", "Add component(s) to your project");
        var componentsArg = new Argument<string[]>("components", "Component name(s) to add");
        command.AddArgument(componentsArg);

        command.SetHandler((components) =>
        {
            AnsiConsole.MarkupLine($"[green]Adding components:[/] {string.Join(", ", components)}");
            AnsiConsole.MarkupLine("[yellow]Coming soon![/]");
        }, componentsArg);

        return command;
    }

    static Command CreateListCommand()
    {
        var command = new Command("list", "List available components");

        command.SetHandler(() =>
        {
            AnsiConsole.MarkupLine("[green]Available components:[/]");
            AnsiConsole.MarkupLine("[yellow]Coming soon![/]");
        });

        return command;
    }
}
```

## Step 3: Test CLI Tool Locally

```bash
# Build the CLI
cd src/ShellUI.CLI
dotnet build

# Run locally
dotnet run -- --help
dotnet run -- init
dotnet run -- add button
dotnet run -- list

# Pack as tool
dotnet pack

# Install locally for testing
dotnet tool install --global --add-source ./bin/Debug ShellUI.CLI --version 0.1.0-alpha

# Test installed tool
shellui --help
shellui init
shellui add button,card

# Uninstall when done testing
dotnet tool uninstall -g ShellUI.CLI
```

## Step 4: Create Core Models

### Create Configuration Model

```csharp
// src/ShellUI.Core/Models/ShellUIConfig.cs
namespace ShellUI.Core.Models;

public class ShellUIConfig
{
    public string Version { get; set; } = "1.0.0";
    public string ProjectType { get; set; } = "blazor-server";
    public string ComponentsPath { get; set; } = "Components/UI";
    public TailwindConfig Tailwind { get; set; } = new();
    public Dictionary<string, string> Aliases { get; set; } = new();
    public List<InstalledComponent> Components { get; set; } = new();
}

public class TailwindConfig
{
    public bool Enabled { get; set; } = true;
    public string Version { get; set; } = "4.x";
    public string BinaryPath { get; set; } = ".shellui/tailwindcss.exe";
}

public class InstalledComponent
{
    public string Name { get; set; } = "";
    public string Version { get; set; } = "";
    public DateTime InstalledAt { get; set; }
    public bool Customized { get; set; }
}
```

### Create Component Metadata Model

```csharp
// src/ShellUI.Core/Models/ComponentMetadata.cs
namespace ShellUI.Core.Models;

public class ComponentMetadata
{
    public string Name { get; set; } = "";
    public string Version { get; set; } = "";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "";
    public List<string> Dependencies { get; set; } = new();
    public List<ComponentFile> Files { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}

public class ComponentFile
{
    public string Path { get; set; } = "";
    public string Type { get; set; } = ""; // "component", "service", "model"
    public string Content { get; set; } = "";
}
```

## Step 5: Create First Component (Button)

### For NuGet Package

```razor
@* src/ShellUI.Components/Components/Button.razor *@
@namespace ShellUI.Components

<button 
    type="@Type"
    disabled="@Disabled"
    class="@CssClass"
    @onclick="HandleClick"
    @attributes="AdditionalAttributes">
    @if (IsLoading)
    {
        <span class="animate-spin">⏳</span>
    }
    @ChildContent
</button>

@code {
    [Parameter] public string Variant { get; set; } = "default";
    [Parameter] public string Size { get; set; } = "md";
    [Parameter] public string Type { get; set; } = "button";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private string CssClass => BuildCssClass();
    
    private string BuildCssClass()
    {
        var classes = new List<string>
        {
            "inline-flex items-center justify-center rounded-md text-sm font-medium",
            "transition-colors focus-visible:outline-none focus-visible:ring-2",
            "focus-visible:ring-ring focus-visible:ring-offset-2",
            "disabled:pointer-events-none disabled:opacity-50"
        };
        
        // Variant classes
        classes.Add(Variant switch
        {
            "default" => "bg-primary text-primary-foreground hover:bg-primary/90",
            "destructive" => "bg-destructive text-destructive-foreground hover:bg-destructive/90",
            "outline" => "border border-input bg-background hover:bg-accent hover:text-accent-foreground",
            "ghost" => "hover:bg-accent hover:text-accent-foreground",
            "link" => "text-primary underline-offset-4 hover:underline",
            _ => "bg-primary text-primary-foreground hover:bg-primary/90"
        });
        
        // Size classes
        classes.Add(Size switch
        {
            "sm" => "h-9 px-3",
            "md" => "h-10 px-4 py-2",
            "lg" => "h-11 px-8",
            _ => "h-10 px-4 py-2"
        });
        
        return string.Join(" ", classes);
    }
    
    private async Task HandleClick(MouseEventArgs args)
    {
        if (!Disabled && !IsLoading)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
```

### For CLI (Template)

```csharp
// src/ShellUI.Templates/Templates/ButtonTemplate.cs
namespace ShellUI.Templates.Templates;

public static class ButtonTemplate
{
    public const string RazorContent = @"
@namespace YourProject.Components.UI

<button 
    type=""@Type""
    disabled=""@Disabled""
    class=""@CssClass""
    @onclick=""HandleClick""
    @attributes=""AdditionalAttributes"">
    @if (IsLoading)
    {
        <span class=""animate-spin"">⏳</span>
    }
    @ChildContent
</button>

@code {
    [Parameter] public string Variant { get; set; } = ""default"";
    [Parameter] public string Size { get; set; } = ""md"";
    [Parameter] public string Type { get; set; } = ""button"";
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private string CssClass => BuildCssClass();
    
    private string BuildCssClass()
    {
        var classes = new List<string>
        {
            ""inline-flex items-center justify-center rounded-md text-sm font-medium"",
            ""transition-colors focus-visible:outline-none focus-visible:ring-2"",
            ""focus-visible:ring-ring focus-visible:ring-offset-2"",
            ""disabled:pointer-events-none disabled:opacity-50""
        };
        
        classes.Add(Variant switch
        {
            ""default"" => ""bg-primary text-primary-foreground hover:bg-primary/90"",
            ""destructive"" => ""bg-destructive text-destructive-foreground hover:bg-destructive/90"",
            ""outline"" => ""border border-input bg-background hover:bg-accent hover:text-accent-foreground"",
            ""ghost"" => ""hover:bg-accent hover:text-accent-foreground"",
            ""link"" => ""text-primary underline-offset-4 hover:underline"",
            _ => ""bg-primary text-primary-foreground hover:bg-primary/90""
        });
        
        classes.Add(Size switch
        {
            ""sm"" => ""h-9 px-3"",
            ""md"" => ""h-10 px-4 py-2"",
            ""lg"" => ""h-11 px-8"",
            _ => ""h-10 px-4 py-2""
        });
        
        return string.Join("" "", classes);
    }
    
    private async Task HandleClick(MouseEventArgs args)
    {
        if (!Disabled && !IsLoading)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}
";

    public static ComponentMetadata Metadata => new()
    {
        Name = "button",
        Version = "1.0.0",
        Description = "Interactive button component with multiple variants",
        Category = "form",
        Dependencies = new List<string>(),
        Files = new List<ComponentFile>
        {
            new ComponentFile
            {
                Path = "Button.razor",
                Type = "component",
                Content = RazorContent
            }
        },
        Tags = new List<string> { "form", "interactive", "essential" }
    };
}
```

## Step 6: Update Example Projects

### Update NET9/BlazorInteractiveServer

```xml
<!-- NET9/BlazorInteractiveServer/BlazorInteractiveServer.csproj -->
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <!-- Remove old package -->
    <!-- <PackageReference Include="Sysinfocus.AspNetCore.Components" Version="0.0.1" /> -->
    
    <!-- Add ShellUI package reference -->
    <ProjectReference Include="../../src/ShellUI.Components/ShellUI.Components.csproj" />
  </ItemGroup>
</Project>
```

```razor
@* NET9/BlazorInteractiveServer/Components/_Imports.razor *@
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop
@using BlazorInteractiveServer
@using BlazorInteractiveServer.Components

@* Remove old *@
@* @using Sysinfocus.AspNetCore.Components *@

@* Add ShellUI *@
@using ShellUI.Components
```

```razor
@* NET9/BlazorInteractiveServer/Components/Pages/Home.razor *@
@page "/"

<PageTitle>Home</PageTitle>

<div class="flex-col">
    <h1>Hello, ShellUI!</h1>
    <Button OnClick="() => showAlert = true">Click Me!</Button>
    @if (showAlert)
    {
        <div class="mt-4 p-4 bg-blue-100 border border-blue-400 rounded">
            <h3 class="font-bold">ShellUI</h3>
            <p>Congratulations! ShellUI is working!</p>
        </div>
    }
</div>

@code
{
    bool showAlert;
}
```

## Step 7: Test Everything

```bash
# Build solution
dotnet build

# Test CLI tool
cd src/ShellUI.CLI
dotnet run -- --help

# Test example app with NuGet package
cd ../../NET9/BlazorInteractiveServer
dotnet run

# Open browser to https://localhost:5001
# Click the button - should work!
```

## Next Actions

### ✅ Task 1: Create Projects (You'll do this first)
Run all the commands in Step 1-3 above

### ⏭️ Task 2: Implement Init Command
- Create `InitCommand.cs`
- Detect project type
- Create `shellui.json`
- Download Tailwind standalone CLI
- Set up folder structure

### ⏭️ Task 3: Implement Add Command
- Create `AddCommand.cs`
- Parse component names (space & comma separated)
- Copy component template to project
- Update imports
- Show success message

### ⏭️ Task 4: Create More Components
- Card
- Alert
- Badge
- Input
- Label

## Verification Checklist

After completing Step 1-7, you should have:

- ✅ Solution with 4 projects
- ✅ CLI tool that runs locally
- ✅ NuGet package with Button component
- ✅ Example app using ShellUI
- ✅ Button component works in browser
- ✅ Ready to implement real commands

## Troubleshooting

### CLI doesn't run
```bash
# Check if built
dotnet build src/ShellUI.CLI

# Run with full path
dotnet run --project src/ShellUI.CLI/ShellUI.CLI.csproj -- --help
```

### Example app doesn't find ShellUI
```bash
# Rebuild solution
dotnet build

# Check project reference
cat NET9/BlazorInteractiveServer/BlazorInteractiveServer.csproj
```

### Button styles don't work
- Tailwind CSS not set up yet (that's Milestone 2!)
- For now, button will work but won't be styled
- That's expected at this stage

---

**Ready?** Run the commands in Step 1 to create the project structure! 🚀

Then we'll implement the real CLI commands step by step.

