using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarJsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-js",
        DisplayName = "Sidebar JS",
        Description = "JavaScript interop module for Sidebar (mobile detection, keyboard shortcuts)",
        Category = ComponentCategory.Layout,
        FilePath = "../../wwwroot/shellui-sidebar.js",
        IsAvailable = false
    };

    public static string Content => @"// ShellUI Sidebar JS Interop Module
// Handles mobile detection, resize events, keyboard shortcuts

export function initSidebar(dotnetRef) {
    const MOBILE_BREAKPOINT = 768;
    const checkMobile = () => window.innerWidth < MOBILE_BREAKPOINT;

    // Initial mobile check
    dotnetRef.invokeMethodAsync('OnMobileChanged', checkMobile());

    // Debounced resize handler
    let resizeTimer;
    const handleResize = () => {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(() => {
            dotnetRef.invokeMethodAsync('OnMobileChanged', checkMobile());
        }, 50);
    };

    // Keyboard shortcut: Ctrl/Cmd + B to toggle sidebar
    const handleKeydown = (e) => {
        if (e.key === 'b' && (e.metaKey || e.ctrlKey)) {
            e.preventDefault();
            dotnetRef.invokeMethodAsync('OnToggle');
        }
    };

    window.addEventListener('resize', handleResize);
    document.addEventListener('keydown', handleKeydown);

    return {
        dispose: () => {
            clearTimeout(resizeTimer);
            window.removeEventListener('resize', handleResize);
            document.removeEventListener('keydown', handleKeydown);
        }
    };
}
";
}
