// Legacy: kept for projects that still load shellui-sidebar.js.

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
