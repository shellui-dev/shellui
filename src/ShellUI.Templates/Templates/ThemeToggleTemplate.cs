using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ThemeToggleTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "theme-toggle",
        DisplayName = "Theme Toggle",
        Description = "Toggle between light and dark mode",
        Category = ComponentCategory.Utility,

        FilePath = "ThemeToggle.razor",
        // Provides ShellUI.addClassToDocument; without it the toggle does nothing.
        Dependencies = new List<string> { "shellui-js" }
    };

    public static string Content => ComponentSource.Read("Components/ThemeToggle.razor");

    public static string ThemeServiceContent => @"using Microsoft.JSInterop;

namespace YourProjectNamespace.Services;

public interface IThemeService
{
    Task<string> GetThemeAsync();
    Task SetThemeAsync(string theme);
    Task ToggleThemeAsync();
    Task InitializeThemeAsync();
}

public class ThemeService : IThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private string _currentTheme = ""dark"";

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetThemeAsync()
    {
        try
        {
            var theme = await _jsRuntime.InvokeAsync<string>(""localStorage.getItem"", ""theme"");
            _currentTheme = string.IsNullOrEmpty(theme) ? ""dark"" : theme;
            return _currentTheme;
        }
        catch
        {
            return _currentTheme;
        }
    }

    public async Task SetThemeAsync(string theme)
    {
        _currentTheme = theme;

        try
        {
            await _jsRuntime.InvokeVoidAsync(""localStorage.setItem"", ""theme"", theme);
            await _jsRuntime.InvokeVoidAsync(
                theme == ""dark"" ? ""ShellUI.addClassToDocument"" : ""ShellUI.removeClassFromDocument"",
                ""dark"");
        }
        catch
        {
            // JSRuntime not available (pre-render)
        }
    }

    public async Task ToggleThemeAsync()
    {
        var currentTheme = await GetThemeAsync();
        var newTheme = currentTheme == ""dark"" ? ""light"" : ""dark"";
        await SetThemeAsync(newTheme);
    }

    public async Task InitializeThemeAsync()
    {
        var theme = await GetThemeAsync();
        await SetThemeAsync(theme);
    }
}
";
}


