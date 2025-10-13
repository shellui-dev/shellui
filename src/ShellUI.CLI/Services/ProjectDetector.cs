using ShellUI.Core.Models;
using System.Xml.Linq;

namespace ShellUI.CLI.Services;

public class ProjectDetector
{
    public static ProjectInfo DetectProject()
    {
        var csprojFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj");
        
        if (csprojFiles.Length == 0)
        {
            throw new Exception("No .csproj file found in current directory. Please run this command from your Blazor project root.");
        }

        var csprojPath = csprojFiles[0];
        var projectName = Path.GetFileNameWithoutExtension(csprojPath);
        
        var doc = XDocument.Load(csprojPath);
        var sdk = doc.Root?.Attribute("Sdk")?.Value ?? "";
        
        var projectType = DetectProjectType(doc, csprojPath);
        var rootNamespace = DetectRootNamespace(doc, projectName);

        return new ProjectInfo
        {
            ProjectPath = csprojPath,
            ProjectName = projectName,
            RootNamespace = rootNamespace,
            ProjectType = projectType
        };
    }

    private static ProjectType DetectProjectType(XDocument doc, string csprojPath)
    {
        var projectDir = Path.GetDirectoryName(csprojPath) ?? "";
        
        // Check for Program.cs patterns
        var programCsPath = Path.Combine(projectDir, "Program.cs");
        if (File.Exists(programCsPath))
        {
            var programContent = File.ReadAllText(programCsPath);
            
            if (programContent.Contains("AddInteractiveServerComponents"))
                return ProjectType.BlazorServer;
            
            if (programContent.Contains("AddInteractiveWebAssemblyComponents"))
                return ProjectType.BlazorWebAssembly;
            
            if (programContent.Contains("AddRazorComponents"))
                return ProjectType.BlazorServerSideRendering;
        }

        // Fallback: check SDK
        var sdk = doc.Root?.Attribute("Sdk")?.Value ?? "";
        if (sdk.Contains("Web"))
            return ProjectType.BlazorServerSideRendering;

        return ProjectType.Unknown;
    }

    private static string DetectRootNamespace(XDocument doc, string projectName)
    {
        var rootNamespace = doc.Root?
            .Descendants("RootNamespace")
            .FirstOrDefault()?.Value;

        return rootNamespace ?? projectName;
    }
}

public class ProjectInfo
{
    public required string ProjectPath { get; set; }
    public required string ProjectName { get; set; }
    public required string RootNamespace { get; set; }
    public ProjectType ProjectType { get; set; }
}

