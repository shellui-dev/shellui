using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class SidebarJsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "sidebar-js",
        DisplayName = "Sidebar JS (legacy)",
        Description = "Legacy JavaScript interop module retained for existing Sidebar projects",
        Category = ComponentCategory.Layout,
        FilePath = "../../wwwroot/shellui-sidebar.js",
        IsAvailable = false
    };

    public static string Content => @"// ShellUI Sidebar JS Interop Module
// Handles mobile detection, resize events, keyboard shortcuts

export function initSidebar(dotnetRef) {
    const MOBILE_BREAKPOINT = 768;
    const checkMobile = () => window.innerWidth < MOBILE_BREAKPOINT;

    dotnetRef.invokeMethodAsync('OnMobileChanged', checkMobile());

    let resizeTimer;
    const handleResize = () => {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(() => {
            dotnetRef.invokeMethodAsync('OnMobileChanged', checkMobile());
        }, 50);
    };

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
