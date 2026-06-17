using ShellUI.CLI.Services;
using Xunit;

namespace ShellUI.Tests;

public class InitBootstrapTests
{
    private const string FreshAppRazor = @"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <base href=""/"" />
    <link rel=""stylesheet"" href=""@Assets[""app.css""]"" />
    <ImportMap />
    <HeadOutlet />
</head>

<body>
    <Routes />
    <script src=""_framework/blazor.web.js""></script>
</body>

</html>
";

    [Fact]
    public void RewriteAppRazor_AddsRenderModeToHeadOutletAndRoutes()
    {
        var result = InitService.RewriteAppRazor(FreshAppRazor);

        Assert.Contains(@"<HeadOutlet @rendermode=""InteractiveServer"" />", result);
        Assert.Contains(@"<Routes @rendermode=""InteractiveServer"" />", result);
    }

    [Fact]
    public void RewriteAppRazor_InjectsThemeBootstrapInHead()
    {
        var result = InitService.RewriteAppRazor(FreshAppRazor);

        Assert.Contains("ShellUI theme bootstrap", result);
        Assert.Contains("classList.add('dark')", result);
        // Theme script must be inside <head>, before </head>.
        var themeIdx = result.IndexOf("ShellUI theme bootstrap");
        var headCloseIdx = result.IndexOf("</head>");
        Assert.True(themeIdx > 0 && themeIdx < headCloseIdx);
    }

    [Fact]
    public void RewriteAppRazor_InjectsShelluiJsBeforeBlazorScript()
    {
        var result = InitService.RewriteAppRazor(FreshAppRazor);

        var shelluiIdx = result.IndexOf(@"<script src=""shellui.js""></script>");
        var blazorIdx = result.IndexOf(@"<script src=""_framework/blazor.web.js""");

        Assert.True(shelluiIdx > 0, "shellui.js script tag was not injected");
        Assert.True(shelluiIdx < blazorIdx, "shellui.js must precede blazor.web.js so window.ShellUI.* is defined before Blazor calls into it");
    }

    [Fact]
    public void RewriteAppRazor_IsIdempotent()
    {
        var once = InitService.RewriteAppRazor(FreshAppRazor);
        var twice = InitService.RewriteAppRazor(once);

        Assert.Equal(once, twice);
    }

    [Fact]
    public void RewriteAppRazor_PreservesExistingRenderMode()
    {
        // If the user (or another tool) already set a different render mode (e.g. Auto),
        // we must not overwrite it.
        const string custom =
            @"<HeadOutlet @rendermode=""InteractiveAuto"" />" + "\n" +
            @"<Routes @rendermode=""InteractiveAuto"" />";

        var result = InitService.RewriteAppRazor(custom);

        Assert.Contains(@"<HeadOutlet @rendermode=""InteractiveAuto"" />", result);
        Assert.Contains(@"<Routes @rendermode=""InteractiveAuto"" />", result);
        Assert.DoesNotContain(@"InteractiveServer", result);
    }

    [Fact]
    public void RewriteWasmIndexHtml_InjectsThemeAndShelluiJs()
    {
        const string indexHtml = @"<!DOCTYPE html>
<html>
<head>
    <title>App</title>
</head>
<body>
    <div id=""app""></div>
    <script src=""_framework/blazor.webassembly.js""></script>
</body>
</html>";

        var result = InitService.RewriteWasmIndexHtml(indexHtml);

        Assert.Contains("ShellUI theme bootstrap", result);
        var shelluiIdx = result.IndexOf(@"<script src=""shellui.js""></script>");
        var blazorIdx = result.IndexOf(@"<script src=""_framework/blazor.webassembly.js""");
        Assert.True(shelluiIdx > 0 && shelluiIdx < blazorIdx);
    }

    [Fact]
    public void RewriteWasmIndexHtml_IsIdempotent()
    {
        const string indexHtml = @"<!DOCTYPE html><html><head></head><body><script src=""_framework/blazor.webassembly.js""></script></body></html>";

        var once = InitService.RewriteWasmIndexHtml(indexHtml);
        var twice = InitService.RewriteWasmIndexHtml(once);

        Assert.Equal(once, twice);
    }
}
