using Microsoft.JSInterop;

namespace ShellUI.Components.Services;

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
    private string _currentTheme = "dark";

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetThemeAsync()
    {
        try
        {
            var theme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme");
            _currentTheme = string.IsNullOrEmpty(theme) ? "dark" : theme;
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
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", theme);
            
            if (theme == "dark")
            {
                await _jsRuntime.InvokeVoidAsync("eval", "document.documentElement.classList.add('dark')");
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("eval", "document.documentElement.classList.remove('dark')");
            }
        }
        catch
        {
            // JSRuntime not available (pre-render)
        }
    }

    public async Task ToggleThemeAsync()
    {
        var currentTheme = await GetThemeAsync();
        var newTheme = currentTheme == "dark" ? "light" : "dark";
        await SetThemeAsync(newTheme);
    }

    public async Task InitializeThemeAsync()
    {
        var theme = await GetThemeAsync();
        await SetThemeAsync(theme);
    }
}

