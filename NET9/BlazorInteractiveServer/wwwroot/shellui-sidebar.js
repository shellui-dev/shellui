// ShellUI Sidebar JS Interop Module
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
