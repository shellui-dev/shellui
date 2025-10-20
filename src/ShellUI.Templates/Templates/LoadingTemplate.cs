namespace ShellUI.Templates.Templates;

public static class LoadingTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "loading",
        Description = "Loading spinner and skeleton components",
        Category = ComponentCategory.Feedback,
        Dependencies = new string[] { }
    };

    public static string Content => @"
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
    <div class=""relative"">
        <div class=""@($""animate-spin rounded-full border-4 border-muted border-t-primary {(Size == ""sm"" ? ""h-6 w-6"" : Size == ""lg"" ? ""h-12 w-12"" : ""h-8 w-8"")} {Class}"")""></div>
        <div class=""@($""absolute inset-0 animate-spin rounded-full border-4 border-transparent border-t-muted {(Size == ""sm"" ? ""h-6 w-6"" : Size == ""lg"" ? ""h-12 w-12"" : ""h-8 w-8"")}"")"" style=""animation-direction: reverse; animation-duration: 1.5s""></div>
    </div>
}

@code {
    [Parameter]
    public string Variant { get; set; } = ""spinner""; // spinner, dots, pulse, ring

    [Parameter]
    public string Size { get; set; } = ""default""; // sm, default, lg

    [Parameter]
    public string Class { get; set; } = """";
}";
}
