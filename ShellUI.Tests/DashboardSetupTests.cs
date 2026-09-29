using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using ShellUI.CLI.Services;
using ShellUI.Core.Models;
using ShellUI.Templates;
using Xunit;

namespace ShellUI.Tests;

public class DashboardSetupTests
{
    [Theory]
    [InlineData("02", "dashboard-02")]
    [InlineData("dashboard-01", "dashboard-01")]
    [InlineData("1", "dashboard-01")]
    [InlineData("None", "none")]
    [InlineData(null, null)]
    public void ParseDashboardOption_AcceptsShortAndLongForms(string? value, string? expected)
    {
        Assert.Equal(expected, DashboardSetup.ParseDashboardOption(value));
    }

    [Fact]
    public void ParseDashboardOption_RejectsUnknownValues()
    {
        Assert.Throws<ArgumentException>(() => DashboardSetup.ParseDashboardOption("03"));
    }

    [Fact]
    public void RewriteDefaultLayout_SwitchesStockRoutes()
    {
        var routes = Fixture("net10", "Routes.razor");
        Assert.Equal("Layout.MainLayout", DashboardSetup.FindDefaultLayout(routes));

        var rewritten = DashboardSetup.RewriteDefaultLayout(routes, "Layout.DashboardLayout02");

        Assert.Contains(@"DefaultLayout=""typeof(Layout.DashboardLayout02)""", rewritten);
        Assert.Contains("NotFoundPage=\"typeof(Pages.NotFound)\"", rewritten);
    }

    [Fact]
    public void FindRoutesFile_AcceptsAuthorizeRouteView()
    {
        var root = Path.Combine(Path.GetTempPath(), "shellui-routes-" + Guid.NewGuid().ToString("N"));
        try
        {
            Write(root, "Components/Routes.razor", @"<AuthorizeRouteView RouteData=""routeData"" DefaultLayout=""typeof(Layout.MainLayout)"" />");

            Assert.Equal(Path.Combine(root, "Components", "Routes.razor"), DashboardSetup.FindRoutesFile(root));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [Fact]
    public void RewriteDefaultLayout_KeepsRazorExpressionPrefix()
    {
        const string wasm = @"<RouteView RouteData=""@routeData"" DefaultLayout=""@typeof(MainLayout)"" />";

        Assert.Equal("MainLayout", DashboardSetup.FindDefaultLayout(wasm));
        Assert.Contains(@"DefaultLayout=""@typeof(X.Y)""", DashboardSetup.RewriteDefaultLayout(wasm, "X.Y"));
    }

    [Theory]
    [InlineData("Components", "Components/Layout", "Layout.DashboardLayout02")]
    [InlineData("", "Components/Layout", "Components.Layout.DashboardLayout02")]
    [InlineData("Components", "Components", "DashboardLayout02")]
    [InlineData("Components", "Shared/Layouts", "global::MyApp.Shared.Layouts.DashboardLayout02")]
    public void LayoutTypeReference_IsRelativeToTheRoutesNamespace(string routesDir, string layoutPath, string expected)
    {
        var cwd = Path.Combine(Path.GetTempPath(), "shellui-app");
        var dir = routesDir.Length == 0 ? cwd : Path.Combine(cwd, routesDir);

        Assert.Equal(expected, DashboardSetup.LayoutTypeReference(dir, cwd, "MyApp", layoutPath, "DashboardLayout02"));
    }

    [Theory]
    [InlineData("net10", "MainLayout.razor")]
    [InlineData("net10", "MainLayout.razor.css")]
    [InlineData("net10", "NavMenu.razor")]
    [InlineData("net10", "NavMenu.razor.css")]
    [InlineData("net8", "MainLayout.razor")]
    [InlineData("net8", "NavMenu.razor")]
    public void IsStockContent_RecognizesTemplateOutput_ForAnyProjectName(string framework, string file)
    {
        foreach (var name in new[] { "Contoso.Shop", "A", "App" })
        {
            var content = Fixture(framework, file).Replace("\r\n", "\n").Replace(DashboardSetup.StockProjectName, name);

            Assert.True(DashboardSetup.IsStockContent(file, content), name);
            Assert.True(DashboardSetup.IsStockContent(file, content.Replace("\n", "\r\n")), name);
        }
    }

    [Theory]
    [InlineData("MainLayout.razor")]
    [InlineData("NavMenu.razor")]
    public void IsStockContent_RejectsModifiedFiles(string file)
    {
        var content = Fixture("net10", file) + "\n<p>custom</p>";

        Assert.False(DashboardSetup.IsStockContent(file, content));
    }

    [Fact]
    public void AddErrorUi_InsertsBeforeBodyOnce()
    {
        var app = Fixture("net10", "App.razor");

        var patched = DashboardSetup.AddErrorUi(app);

        Assert.Contains(@"id=""blazor-error-ui""", patched);
        Assert.True(patched.IndexOf("blazor-error-ui", StringComparison.Ordinal) < patched.IndexOf("</body>", StringComparison.Ordinal));
        Assert.Contains("class=\"dismiss", patched);
        Assert.Contains("class=\"reload", patched);
        Assert.Equal(patched, DashboardSetup.AddErrorUi(patched));
    }

    [Theory]
    [InlineData("@page \"/not-found\"\r\n@layout MainLayout\r\n\r\n<h3>Not Found</h3>", "@page \"/not-found\"\r\n@layout DashboardLayout02\r\n\r\n<h3>Not Found</h3>")]
    [InlineData("@layout Layout.MainLayout\n", "@layout Layout.DashboardLayout02\n")]
    [InlineData("@layout MainLayoutV2\n", "@layout MainLayoutV2\n")]
    [InlineData("<p>MainLayout</p>", "<p>MainLayout</p>")]
    public void RetargetLayoutDirective_OnlyTouchesMainLayoutDirectives(string content, string expected)
    {
        Assert.Equal(expected, DashboardSetup.RetargetLayoutDirective(content, "MainLayout", "DashboardLayout02"));
    }

    [Fact]
    public void RetargetLayoutDirective_FollowsADashboardSwitch()
    {
        Assert.Equal("@layout DashboardLayout01\n", DashboardSetup.RetargetLayoutDirective("@layout DashboardLayout02\n", "DashboardLayout02", "DashboardLayout01"));
    }

    [Theory]
    [InlineData("/", true)]
    [InlineData("/counter", true)]
    [InlineData("/items/{id:int}", false)]
    [InlineData("/Error", false)]
    [InlineData("/not-found", false)]
    [InlineData("/Account/Login", false)]
    [InlineData("/accounting", true)]
    public void IsNavigableRoute_SkipsParameterizedAndSystemPages(string href, bool expected)
    {
        Assert.Equal(expected, DashboardSetup.IsNavigableRoute(href));
    }

    [Fact]
    public void ScanPages_BuildsLinksFromPageDirectives()
    {
        var root = Path.Combine(Path.GetTempPath(), "shellui-scan-" + Guid.NewGuid().ToString("N"));
        try
        {
            Write(root, "Components/Pages/Home.razor", "@page \"/\"\n<PageTitle>Home</PageTitle>");
            Write(root, "Components/Pages/Counter.razor", "@page \"/counter\"\n@rendermode InteractiveServer\n<PageTitle>Counter</PageTitle>");
            Write(root, "Components/Pages/OrderHistory.razor", "@page \"/orders\"");
            Write(root, "Components/Pages/Error.razor", "@page \"/Error\"");
            Write(root, "Components/Pages/Item.razor", "@page \"/items/{id}\"");
            Write(root, "Components/Account/Pages/Login.razor", "@page \"/Account/Login\"");
            Write(root, "Components/UI/Demo.razor", "@page \"/ui-demo\"");
            Write(root, "obj/Debug/Stale.razor", "@page \"/stale\"");

            var links = DashboardSetup.ScanPages(root, new ShellUIConfig());

            Assert.Equal(new[] { "/", "/counter", "/orders" }, links.Select(l => l.Href));
            Assert.Equal(new[] { "Home", "Counter", "Order History" }, links.Select(l => l.Title));
            Assert.Equal(new[] { "home", "counter", "page" }, links.Select(l => l.Icon));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [Fact]
    public void AppSidebarTemplate_LinksAreRewrittenFromPages()
    {
        var sidebar = ComponentRegistry.GetComponentContent("app-sidebar")!;
        var links = new List<DashboardSetup.PageLink>
        {
            new("Home", "/", "home"),
            new("Weather \"Now\"", "/weather", "weather")
        };

        var rewritten = DashboardSetup.RewriteSidebarLinks(sidebar, links);

        Assert.NotNull(rewritten);
        Assert.Contains(@"new(""Weather \""Now\"""", ""/weather"", ""weather""),", rewritten);
        Assert.DoesNotContain("/settings", rewritten);
        Assert.DoesNotContain("\"/dashboard\"", rewritten);
        Assert.Null(DashboardSetup.RewriteSidebarLinks(rewritten!, links));
    }

    private static void Write(string root, string relative, string content)
    {
        var path = Path.Combine(root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content);
    }

    private static string Fixture(string framework, string file, [CallerFilePath] string thisFile = "") =>
        File.ReadAllText(Path.Combine(Path.GetDirectoryName(thisFile)!, "Fixtures", "StockBlazor", framework, file + ".txt"));
}
