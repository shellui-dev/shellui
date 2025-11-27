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
        Version = "0.1.0",
        FilePath = "FileUpload.razor",
        Dependencies = new List<string>(),
        Tags = new[] { "file", "upload", "drag", "drop", "validation", "storage" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.JSInterop

<div class=""@($""relative w-full {Class}"")"" @ref=""containerRef"" id=""@_dropZoneId"">
    <label for=""@_inputId"" class=""@GetDropZoneClasses()"" 
     @ondragover=""HandleDragOver"" 
     @ondragleave=""HandleDragLeave""
     @ondrop=""HandleDrop""
     @ondragover:preventDefault=""true""
     @ondrop:preventDefault=""true"">
        <div class=""text-center pointer-events-none"">
            <span class=""material-symbols-outlined mx-auto text-5xl text-muted-foreground block"">cloud_upload</span>
            <div class=""mt-4"">
                <p class=""text-sm text-foreground font-medium"">@Text</p>
                @if (Multiple && AllowedFileCount > 1)
                {
                    <p class=""text-xs text-muted-foreground mt-1"">Select up to @AllowedFileCount files</p>
                }
            </div>
        </div>
    </label>

    <InputFile id=""@_inputId""
               @ref=""fileInputRef"" 
               OnChange=""HandleFileSelection"" 
               multiple=""@Multiple""
               accept=""@GetAcceptString()""
               class=""hidden"" />

    @ChildContent
</div>

@code {
    [Parameter] public string Text { get; set; } = ""Drag and drop or click here to upload files"";
    [Parameter] public string[]? AcceptedFileTypes { get; set; }
    [Parameter] public int AllowedFileCount { get; set; } = 1;
    [Parameter] public bool Multiple { get; set; } = false;
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }

    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    private ElementReference containerRef;
    private InputFile? fileInputRef;
    private string _inputId = $""file-input-{Guid.NewGuid():N}"";
    private string _dropZoneId = $""drop-zone-{Guid.NewGuid():N}"";
    private bool _isDragOver;

    private string GetDropZoneClasses()
    {
        var baseClasses = ""block border-2 border-dashed rounded-lg p-8 cursor-pointer transition-all duration-200 border-border bg-background"";
        
        if (_isDragOver)
            baseClasses += "" border-primary bg-primary/10 scale-[1.02]"";
        else if (Disabled)
            baseClasses += "" opacity-50 cursor-not-allowed"";
        else
            baseClasses += "" hover:border-primary/50 hover:bg-muted/50 active:scale-[0.98]"";

        return baseClasses;
    }

    private string GetAcceptString()
    {
        if (AcceptedFileTypes?.Any() != true) return string.Empty;
        return string.Join("","", AcceptedFileTypes);
    }

    private void HandleDragOver(DragEventArgs e)
    {
        if (Disabled) return;
        _isDragOver = true;
        StateHasChanged();
    }

    private void HandleDragLeave(DragEventArgs e)
    {
        if (Disabled) return;
        _isDragOver = false;
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !Disabled)
        {
            await JSRuntime.InvokeVoidAsync(""ShellUI.setupFileDrop"", _dropZoneId, _inputId);
        }
    }

    private void HandleDrop(DragEventArgs e)
    {
        if (Disabled) return;
        _isDragOver = false;
        StateHasChanged();
    }

    private async Task HandleFileSelection(InputFileChangeEventArgs e)
    {
        if (Disabled || !OnChange.HasDelegate) return;
        await OnChange.InvokeAsync(e);
    }
}
";
}
