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
        Version = "0.1.0",
        FilePath = "ThemeToggle.razor",
        Dependencies = new List<string>()
    };

    public static string Content => @"@using Microsoft.JSInterop
@inject IJSRuntime JSRuntime

<button 
    @onclick=""ToggleTheme"" 
    class=""inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 hover:bg-accent hover:text-accent-foreground h-9 w-9""
    title=""Toggle theme"">
    @if (_isDark)
    {
        <!-- Sun icon (light mode) -->
        <svg class=""h-5 w-5"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z"" />
        </svg>
    }
    else
    {
        <!-- Moon icon (dark mode) -->
        <svg class=""h-5 w-5"" xmlns=""http://www.w3.org/2000/svg"" fill=""none"" viewBox=""0 0 24 24"" stroke=""currentColor"">
            <path stroke-linecap=""round"" stroke-linejoin=""round"" stroke-width=""2"" d=""M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z"" />
        </svg>
    }
</button>

@code {
    private bool _isDark = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var theme = await JSRuntime.InvokeAsync<string>(""localStorage.getItem"", ""theme"");
            _isDark = string.IsNullOrEmpty(theme) ? true : theme == ""dark"";
        }
        catch
        {
            _isDark = true;
        }
    }

    private async Task ToggleTheme()
    {
        _isDark = !_isDark;
        var theme = _isDark ? ""dark"" : ""light"";
        
        try
        {
            await JSRuntime.InvokeVoidAsync(""localStorage.setItem"", ""theme"", theme);
            
            if (_isDark)
            {
                await JSRuntime.InvokeVoidAsync(""eval"", ""document.documentElement.classList.add('dark')"");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync(""eval"", ""document.documentElement.classList.remove('dark')"");
            }
        }
        catch
        {
            // JSRuntime not available (pre-render)
        }
    }
}
";

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
            
            if (theme == ""dark"")
            {
                await _jsRuntime.InvokeVoidAsync(""eval"", ""document.documentElement.classList.add('dark')"");
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync(""eval"", ""document.documentElement.classList.remove('dark')"");
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

