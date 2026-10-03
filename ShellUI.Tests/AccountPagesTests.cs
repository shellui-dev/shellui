using System;
using System.IO;
using System.Runtime.CompilerServices;
using ShellUI.CLI.Services;
using Xunit;

namespace ShellUI.Tests;

public class AccountPagesTests
{
    [Theory]
    [InlineData("net10", "Pages/Login.razor")]
    [InlineData("net10", "Pages/Manage/Email.razor")]
    [InlineData("net10", "Shared/ManageLayout.razor")]
    [InlineData("net10", "Shared/ManageNavMenu.razor")]
    [InlineData("net8", "Pages/Login.razor")]
    [InlineData("net8", "Shared/ManageLayout.razor")]
    public void Restyle_RemovesBootstrapFromStockPages(string framework, string file)
    {
        var content = Fixture(framework, file);
        Assert.True(AccountPages.IsStock(file, content, DashboardSetup.StockProjectName));

        var restyled = AccountPages.Restyle(content);

        Assert.False(AccountPages.UsesBootstrap(restyled));
        Assert.DoesNotContain("class=\"row\"", restyled);
        Assert.DoesNotContain("<h1>", restyled);
        Assert.DoesNotContain("<hr />", restyled);
        Assert.False(AccountPages.IsStock(file, restyled, DashboardSetup.StockProjectName));
    }

    [Theory]
    [InlineData("MyApp")]
    [InlineData("my_app")]
    public void IsStock_IgnoresTheProjectNamespace(string rootNamespace)
    {
        var content = Fixture("net10", "Pages/Login.razor").Replace(DashboardSetup.StockProjectName, rootNamespace);

        Assert.True(AccountPages.IsStock("Pages/Login.razor", content, rootNamespace));
        Assert.False(AccountPages.IsStock("Pages/Login.razor", content + "\n<p>mine</p>", rootNamespace));
    }

    [Fact]
    public void Restyle_KeepsRazorExpressionsInsideTags()
    {
        var restyled = AccountPages.Restyle(Fixture("net10", "Pages/Login.razor"));

        Assert.Contains("<a class=\"font-medium underline underline-offset-4\" href=\"@(NavigationManager.GetUriWithQueryParameters(\"Account/Register\", new Dictionary<string, object?> { [\"ReturnUrl\"] = ReturnUrl }))\">", restyled);
        Assert.Contains("@bind-Value=\"Input.Email\"", restyled);
        Assert.Contains("OnSubmit=\"LoginUser\"", restyled);
    }

    [Fact]
    public void Restyle_StatusMessageUsesThemeColors()
    {
        var restyled = AccountPages.Restyle(Fixture("net10", "Shared/StatusMessage.razor"));

        Assert.DoesNotContain("alert-@", restyled);
        Assert.Contains("\"text-destructive\"", restyled);
        Assert.Contains("@statusMessageClass\"", restyled);
    }

    [Theory]
    [InlineData("btn btn-primary", "bg-primary", "h-10 px-4 py-2")]
    [InlineData("w-100 btn btn-lg btn-primary", "w-full", "h-11 px-8")]
    [InlineData("btn btn-danger", "bg-destructive", "h-10 px-4 py-2")]
    public void MapClasses_BuildsShellUIButtons(string bootstrap, string variant, string size)
    {
        var mapped = AccountPages.MapClasses(bootstrap);

        Assert.Contains(variant, mapped);
        Assert.Contains(size, mapped);
        Assert.DoesNotContain("btn", mapped.Split(' '));
        Assert.DoesNotContain("w-100", mapped.Split(' '));
    }

    [Fact]
    public void Apply_RestylesStockAccountPagesAndReportsEditedOnes()
    {
        var root = Path.Combine(Path.GetTempPath(), "shellui-account-" + Guid.NewGuid().ToString("N"));
        try
        {
            var login = Fixture("net10", "Pages/Login.razor").Replace(DashboardSetup.StockProjectName, "MyApp");
            Write(root, "Components/Account/Pages/Login.razor", login);
            Write(root, "Components/Account/Shared/ManageLayout.razor", Fixture("net10", "Shared/ManageLayout.razor").Replace(DashboardSetup.StockProjectName, "MyApp") + "\n<p>mine</p>");
            Write(root, "Components/Pages/Counter.razor", "<button class=\"btn btn-primary\"></button>");

            var result = AccountPages.Apply(root, "MyApp");

            Assert.Equal(new[] { "Components/Account/Pages/Login.razor" }, result.Restyled);
            Assert.False(AccountPages.UsesBootstrap(File.ReadAllText(Path.Combine(root, "Components/Account/Pages/Login.razor"))));
            Assert.Contains(result.Notes, n => n.Contains("Components/Account/Shared/ManageLayout.razor") && n.Contains("modified"));
            Assert.Single(result.Notes);
            Assert.Contains("btn btn-primary", File.ReadAllText(Path.Combine(root, "Components/Pages/Counter.razor")));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    private static void Write(string root, string relative, string content)
    {
        var path = Path.Combine(root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, content);
    }

    private static string Fixture(string framework, string file, [CallerFilePath] string thisFile = "") =>
        File.ReadAllText(Path.Combine(Path.GetDirectoryName(thisFile)!, "Fixtures", "StockBlazor", framework, "Account", file + ".txt"));
}
