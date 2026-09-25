using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ShellUIJsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "shellui-js",
        DisplayName = "ShellUI JS",
        Description = "Global JavaScript utilities for ShellUI interactions and sidebar interop",
        Category = ComponentCategory.Utility,
        FilePath = "../../wwwroot/shellui.js",
        IsAvailable = false
    };

    public static string Content => """
window.ShellUI = window.ShellUI || {};

Object.assign(window.ShellUI, {
    focusElement: function (elementId) {
        const element = document.getElementById(elementId);
        if (element) {
            element.focus();
        }
    },
    
    focusBody: function () {
        document.body.focus();
    },
    
    addClassToDocument: function (className) {
        document.documentElement.classList.add(className);
    },
    
    removeClassFromDocument: function (className) {
        document.documentElement.classList.remove(className);
    },
    
    toggleClassOnDocument: function (className) {
        document.documentElement.classList.toggle(className);
    },
    
    assignFilesToInput: function (inputElement, dataTransfer) {
        if (!inputElement) {
            return false;
        }
        
        try {
            const dt = new DataTransfer();
            let files;
            if (dataTransfer && dataTransfer.files) {
                files = dataTransfer.files;
            } else {
                return false;
            }
            for (let i = 0; i < files.length; i++) {
                dt.items.add(files[i]);
            }
            inputElement.files = dt.files;
            const event = new Event('change', { bubbles: true });
            inputElement.dispatchEvent(event);
            return true;
        } catch (error) {
            console.error('Error assigning files to input:', error);
            return false;
        }
    },
    
    setupFileDrop: function (dropZoneId, inputElementId) {
        const dropZone = document.getElementById(dropZoneId);
        const inputElement = document.getElementById(inputElementId);
        if (!dropZone || !inputElement) return false;
        dropZone.removeEventListener('drop', handleDrop);
        function handleDrop(e) {
            e.preventDefault();
            e.stopPropagation();
            const dataTransfer = e.dataTransfer;
            if (!dataTransfer || !dataTransfer.files || dataTransfer.files.length === 0) return;
            try {
                const dt = new DataTransfer();
                const files = dataTransfer.files;
                for (let i = 0; i < files.length; i++) dt.items.add(files[i]);
                inputElement.files = dt.files;
                inputElement.dispatchEvent(new Event('change', { bubbles: true }));
            } catch (error) { console.error('Error handling drop files:', error); }
        }
        dropZone.addEventListener('drop', handleDrop);
        return true;
    },
    
    openUrl: function (url, target) {
        if (window.open) window.open(url, target || '_blank');
        else console.error('window.open is not available');
    },

    copyToClipboard: function (text) {
        return navigator.clipboard.writeText(text);
    },

    _shortcutHandlers: new Map(),

    registerShortcut: function (handle, key, ctrl, meta, shift, alt, dotNetRef) {
        const listener = (e) => {
            if (e.key.toLowerCase() !== key.toLowerCase()) return;
            if (ctrl && !e.ctrlKey) return;
            if (meta && !e.metaKey) return;
            if (!ctrl && !meta && (e.ctrlKey || e.metaKey)) return;
            if (shift !== e.shiftKey) return;
            if (alt !== e.altKey) return;
            e.preventDefault();
            dotNetRef.invokeMethodAsync("OnShortcut");
        };
        window.addEventListener("keydown", listener);
        this._shortcutHandlers.set(handle, listener);
    },

    unregisterShortcut: function (handle) {
        const listener = this._shortcutHandlers.get(handle);
        if (listener) {
            window.removeEventListener("keydown", listener);
            this._shortcutHandlers.delete(handle);
        }
    },

    _scrollLockCount: 0,
    _originalOverflow: "",
    lockBodyScroll: function () {
        if (this._scrollLockCount === 0) {
            this._originalOverflow = document.body.style.overflow;
            document.body.style.overflow = "hidden";
        }
        this._scrollLockCount++;
    },
    unlockBodyScroll: function () {
        if (this._scrollLockCount === 0) return;
        this._scrollLockCount--;
        if (this._scrollLockCount === 0) {
            document.body.style.overflow = this._originalOverflow;
        }
    },

    _dismissHandlers: new Map(),
    onDismissEvents: function (handle, dotNetRef) {
        const listener = () => dotNetRef.invokeMethodAsync("OnDismissEvent");
        window.addEventListener("scroll", listener);
        window.addEventListener("resize", listener);
        this._dismissHandlers.set(handle, listener);
    },
    offDismissEvents: function (handle) {
        const listener = this._dismissHandlers.get(handle);
        if (listener) {
            window.removeEventListener("scroll", listener);
            window.removeEventListener("resize", listener);
            this._dismissHandlers.delete(handle);
        }
    },

    // Sidebar mobile detection + Ctrl/Cmd+B shortcut. Lives here (rather than a
    // dynamically-imported shellui-sidebar.js) because a relative import resolves
    // against the page URL, which breaks the moment SidebarProvider is compiled
    // into a consumer's own Razor Class Library instead of installed straight into
    // the host app — the file is then served from _content/<Library>/ and the
    // import 404s silently (caught). shellui.js is already loaded globally via the
    // host-controlled script tag, so no per-component import is needed.
    _sidebarHandlers: new Map(),
    initSidebar: function (handle, dotNetRef) {
        this.disposeSidebar(handle);

        const media = window.matchMedia("(max-width: 767px)");
        const notify = (isMobile) => {
            dotNetRef.invokeMethodAsync("OnMobileChanged", isMobile).catch(() => {});
        };
        const handleChange = () => notify(media.matches);
        const handleKeydown = (e) => {
            if ((e.key === "b" || e.key === "B") && (e.metaKey || e.ctrlKey)) {
                e.preventDefault();
                dotNetRef.invokeMethodAsync("OnToggle").catch(() => {});
            }
        };

        notify(media.matches);
        if (media.addEventListener) {
            media.addEventListener("change", handleChange);
        } else {
            media.addListener(handleChange);
        }
        document.addEventListener("keydown", handleKeydown);
        this._sidebarHandlers.set(handle, { media, handleChange, handleKeydown });
    },
    disposeSidebar: function (handle) {
        const h = this._sidebarHandlers.get(handle);
        if (!h) return;
        if (h.media.removeEventListener) {
            h.media.removeEventListener("change", h.handleChange);
        } else {
            h.media.removeListener(h.handleChange);
        }
        document.removeEventListener("keydown", h.handleKeydown);
        this._sidebarHandlers.delete(handle);
    }
});
""";
}
