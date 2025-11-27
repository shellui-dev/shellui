namespace ShellUI.Templates.Templates;

public static class LoadingTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "loading",
        Description = "Loading spinner and skeleton components with multiple variants (requires CSS keyframes - see component comments)",
        Category = ComponentCategory.Feedback,
        Dependencies = new string[] { }
    };

    public static string Content => @"
@* 
 * IMPORTANT: For bars, bars-vertical, bars-pulse, and orbit variants to work,
 * add these CSS keyframes to your input.css file:
 *
 * @keyframes bars {
 *   0%, 100% { transform: scaleY(0.4); opacity: 0.7; }
 *   50% { transform: scaleY(1); opacity: 1; }
 * }
 *
 * @keyframes bars-vertical {
 *   0%, 100% { transform: scaleY(0.4); opacity: 0.7; }
 *   50% { transform: scaleY(1); opacity: 1; }
 * }
 *
 * @keyframes bars-pulse {
 *   0%, 100% { transform: scaleY(0.3); opacity: 0.5; }
 *   50% { transform: scaleY(1); opacity: 1; }
 * }
 *
 * @keyframes orbit {
 *   0% { transform: translate(-50%, 0) rotate(0deg) translateX(calc((var(--size, 1rem) - 0.5rem) / 2)) rotate(0deg); }
 *   100% { transform: translate(-50%, 0) rotate(360deg) translateX(calc((var(--size, 1rem) - 0.5rem) / 2)) rotate(-360deg); }
 * }
 *@
@if (Variant == ""spinner"")
{
    <div class=""@($""animate-spin rounded-full border-2 border-muted border-t-primary {(Size == ""sm"" ? ""h-4 w-4"" : Size == ""lg"" ? ""h-8 w-8"" : ""h-6 w-6"")} {Class}"")""></div>
}
else if (Variant == ""dots"")
{
    <div class=""flex space-x-1"">
        <div class=""@($""animate-bounce rounded-full bg-primary {(Size == ""sm"" ? ""h-2 w-2"" : Size == ""lg"" ? ""h-4 w-4"" : ""h-3 w-3"")} {Class}"")"" style=""animation-delay: 0ms""></div>
        <div class=""@($""animate-bounce rounded-full bg-primary {(Size == ""sm"" ? ""h-2 w-2"" : Size == ""lg"" ? ""h-4 w-4"" : ""h-3 w-3"")} {Class}"")"" style=""animation-delay: 150ms""></div>
        <div class=""@($""animate-bounce rounded-full bg-primary {(Size == ""sm"" ? ""h-2 w-2"" : Size == ""lg"" ? ""h-4 w-4"" : ""h-3 w-3"")} {Class}"")"" style=""animation-delay: 300ms""></div>
    </div>
}
else if (Variant == ""pulse"")
{
    <div class=""@($""animate-pulse rounded-md bg-muted {(Size == ""sm"" ? ""h-4 w-16"" : Size == ""lg"" ? ""h-6 w-24"" : ""h-5 w-20"")} {Class}"")""></div>
}
else if (Variant == ""ring"")
{
    <div class=""@($""animate-spin rounded-full border-4 border-muted border-t-primary border-r-primary/50 {(Size == ""sm"" ? ""h-6 w-6"" : Size == ""lg"" ? ""h-12 w-12"" : ""h-8 w-8"")} {Class}"")""></div>
}
else if (Variant == ""bars"")
{
    <div class=""flex items-end gap-1"">
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-3 w-1"" : Size == ""lg"" ? ""h-6 w-2"" : ""h-4 w-1.5"")} {Class}"")"" style=""animation: bars 1.2s ease-in-out infinite; animation-delay: 0s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-4 w-1"" : Size == ""lg"" ? ""h-8 w-2"" : ""h-6 w-1.5"")} {Class}"")"" style=""animation: bars 1.2s ease-in-out infinite; animation-delay: 0.15s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-5 w-1"" : Size == ""lg"" ? ""h-10 w-2"" : ""h-8 w-1.5"")} {Class}"")"" style=""animation: bars 1.2s ease-in-out infinite; animation-delay: 0.3s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-4 w-1"" : Size == ""lg"" ? ""h-8 w-2"" : ""h-6 w-1.5"")} {Class}"")"" style=""animation: bars 1.2s ease-in-out infinite; animation-delay: 0.45s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-3 w-1"" : Size == ""lg"" ? ""h-6 w-2"" : ""h-4 w-1.5"")} {Class}"")"" style=""animation: bars 1.2s ease-in-out infinite; animation-delay: 0.6s;""></div>
    </div>
}
else if (Variant == ""bars-vertical"")
{
    <div class=""flex items-center gap-1"">
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-3 w-1"" : Size == ""lg"" ? ""h-6 w-2"" : ""h-4 w-1.5"")} {Class}"")"" style=""animation: bars-vertical 1.2s ease-in-out infinite; animation-delay: 0s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-4 w-1"" : Size == ""lg"" ? ""h-8 w-2"" : ""h-6 w-1.5"")} {Class}"")"" style=""animation: bars-vertical 1.2s ease-in-out infinite; animation-delay: 0.15s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-5 w-1"" : Size == ""lg"" ? ""h-10 w-2"" : ""h-8 w-1.5"")} {Class}"")"" style=""animation: bars-vertical 1.2s ease-in-out infinite; animation-delay: 0.3s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-4 w-1"" : Size == ""lg"" ? ""h-8 w-2"" : ""h-6 w-1.5"")} {Class}"")"" style=""animation: bars-vertical 1.2s ease-in-out infinite; animation-delay: 0.45s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-3 w-1"" : Size == ""lg"" ? ""h-6 w-2"" : ""h-4 w-1.5"")} {Class}"")"" style=""animation: bars-vertical 1.2s ease-in-out infinite; animation-delay: 0.6s;""></div>
    </div>
}
else if (Variant == ""bars-pulse"")
{
    <div class=""flex items-center gap-1"">
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-3 w-1"" : Size == ""lg"" ? ""h-6 w-2"" : ""h-4 w-1.5"")} {Class}"")"" style=""animation: bars-pulse 1.2s ease-in-out infinite; animation-delay: 0s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-4 w-1"" : Size == ""lg"" ? ""h-8 w-2"" : ""h-6 w-1.5"")} {Class}"")"" style=""animation: bars-pulse 1.2s ease-in-out infinite; animation-delay: 0.15s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-5 w-1"" : Size == ""lg"" ? ""h-10 w-2"" : ""h-8 w-1.5"")} {Class}"")"" style=""animation: bars-pulse 1.2s ease-in-out infinite; animation-delay: 0.3s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-4 w-1"" : Size == ""lg"" ? ""h-8 w-2"" : ""h-6 w-1.5"")} {Class}"")"" style=""animation: bars-pulse 1.2s ease-in-out infinite; animation-delay: 0.45s;""></div>
        <div class=""@($""bg-primary rounded-sm {(Size == ""sm"" ? ""h-3 w-1"" : Size == ""lg"" ? ""h-6 w-2"" : ""h-4 w-1.5"")} {Class}"")"" style=""animation: bars-pulse 1.2s ease-in-out infinite; animation-delay: 0.6s;""></div>
    </div>
}
else if (Variant == ""grid"")
{
    <div class=""@($""grid grid-cols-3 gap-1 {(Size == ""sm"" ? ""w-4 h-4"" : Size == ""lg"" ? ""w-8 h-8"" : ""w-6 h-6"")} {Class}"")"">
        @for (int i = 0; i < 9; i++)
        {
            <div class=""bg-primary rounded-sm animate-pulse"" style=""animation-delay: @(i * 0.1)s; animation-duration: 1.2s;""></div>
        }
    </div>
}
else if (Variant == ""orbit"")
{
    <div class=""@($""relative {(Size == ""sm"" ? ""h-6 w-6"" : Size == ""lg"" ? ""h-12 w-12"" : ""h-8 w-8"")} {Class}"")"">
        <div class=""absolute inset-0 rounded-full border-2 border-primary/20""></div>
        <div class=""@($""absolute top-0 left-1/2 -translate-x-1/2 rounded-full bg-primary animate-spin {(Size == ""sm"" ? ""h-1.5 w-1.5"" : Size == ""lg"" ? ""h-3 w-3"" : ""h-2 w-2"")}"")"" style=""transform-origin: center calc(50% + @(Size == ""sm"" ? ""0.75rem"" : Size == ""lg"" ? ""1.5rem"" : ""1rem""));""></div>
    </div>
}

@code {
    [Parameter]
    public string Variant { get; set; } = ""spinner"";

    [Parameter]
    public string Size { get; set; } = ""default"";

    [Parameter]
    public string Class { get; set; } = """";
}";
}
