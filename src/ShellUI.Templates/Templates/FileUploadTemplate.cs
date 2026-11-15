using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public class FileUploadTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "file-upload",
        DisplayName = "File Upload",
        Description = "Simple file upload component with drag & drop support",
        Category = ComponentCategory.Form,

        FilePath = "FileUpload.razor",
        Dependencies = new List<string> { "shellui-js" },
        Tags = new List<string> { "file", "upload", "drag", "drop", "validation", "storage" }
    };

    public static string Content => ComponentSource.Read("Components/FileUpload.razor");
}

