using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ImageViewerTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "image-viewer",
        DisplayName = "Image Viewer",
        Description = "Image thumbnail that opens a zoomable lightbox",
        Category = ComponentCategory.Media,
        FilePath = "ImageViewer.razor",
        Dependencies = new List<string>(),
        Tags = new List<string> { "image", "lightbox", "media", "zoom" }
    };

    public static string Content => @"@namespace YourProjectNamespace.Components.UI
@using System.Globalization

<button type=""button"" @onclick=""Open"" aria-label=""@($""View {Alt}"")""
        class=""@Shell.Cn(""group relative block overflow-hidden rounded-md focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring"", Class)"">
    <img src=""@(ThumbnailSrc ?? Src)"" alt=""@Alt"" loading=""lazy""
         class=""@Shell.Cn(""h-full w-full object-cover transition-transform duration-200 group-hover:scale-105"", ImageClass)""
         @attributes=""AdditionalAttributes"" />
</button>

@if (_open)
{
    <div @ref=""_overlay"" tabindex=""-1"" role=""dialog"" aria-modal=""true"" aria-label=""@Alt""
         @onkeydown=""HandleKeyDown"" @onclick=""Close""
         class=""fixed inset-0 z-50 flex items-center justify-center bg-black/80 outline-none"">
        <div class=""absolute right-4 top-4 flex gap-2"" @onclick:stopPropagation=""true"">
            <button type=""button"" aria-label=""Zoom out"" disabled=""@(_scale <= MinScale)"" @onclick=""ZoomOut"" class=""@ControlClass"">
                <svg class=""h-4 w-4"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor""><path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M5 12h14"" /></svg>
            </button>
            <button type=""button"" aria-label=""Reset zoom"" @onclick=""ResetZoom"" class=""@Shell.Cn(ControlClass, ""w-auto px-2 text-xs tabular-nums"")"">@($""{_scale * 100:0}%"")</button>
            <button type=""button"" aria-label=""Zoom in"" disabled=""@(_scale >= MaxScale)"" @onclick=""ZoomIn"" class=""@ControlClass"">
                <svg class=""h-4 w-4"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor""><path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 5v14M5 12h14"" /></svg>
            </button>
            <button type=""button"" aria-label=""Close"" @onclick=""Close"" class=""@ControlClass"">
                <svg class=""h-4 w-4"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor""><path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M6 18L18 6M6 6l12 12"" /></svg>
            </button>
        </div>
        <img src=""@Src"" alt=""@Alt"" @onclick:stopPropagation=""true""
             style=""transform: scale(@_scale.ToString(CultureInfo.InvariantCulture))""
             class=""max-h-[90vh] max-w-[90vw] select-none object-contain transition-transform duration-200"" />
    </div>
}

@code {
    [Parameter] public string Src { get; set; } = """";
    [Parameter] public string? ThumbnailSrc { get; set; }
    [Parameter] public string Alt { get; set; } = """";
    [Parameter] public string? ImageClass { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private const double MinScale = 0.5;
    private const double MaxScale = 4;
    private const string ControlClass = ""inline-flex h-9 w-9 items-center justify-center rounded-md bg-white/10 text-white transition-colors hover:bg-white/20 disabled:pointer-events-none disabled:opacity-40"";

    private ElementReference _overlay;
    private bool _open;
    private bool _focusPending;
    private double _scale = 1;

    private void Open()
    {
        _open = true;
        _scale = 1;
        _focusPending = true;
    }

    private void Close() => _open = false;
    private void ZoomIn() => _scale = Math.Min(MaxScale, _scale + 0.25);
    private void ZoomOut() => _scale = Math.Max(MinScale, _scale - 0.25);
    private void ResetZoom() => _scale = 1;

    private void HandleKeyDown(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case ""Escape"": Close(); break;
            case ""+"": case ""="": ZoomIn(); break;
            case ""-"": ZoomOut(); break;
            case ""0"": ResetZoom(); break;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!_focusPending) return;
        _focusPending = false;
        await _overlay.FocusAsync();
    }
}
";
}
