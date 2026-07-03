window.ShellUI = window.ShellUI || {};

const shortcutHandlers = new Map();

Object.assign(window.ShellUI, {
    copyToClipboard: function (text) {
        return navigator.clipboard.writeText(text);
    },

    focusElement: function (elementId) {
        const element = document.getElementById(elementId);
        if (element) element.focus();
    },

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
        shortcutHandlers.set(handle, listener);
    },

    unregisterShortcut: function (handle) {
        const listener = shortcutHandlers.get(handle);
        if (listener) {
            window.removeEventListener("keydown", listener);
            shortcutHandlers.delete(handle);
        }
    }
});

// ES module re-exports so consumers who import this file dynamically still work.
export function copyToClipboard(text) { return window.ShellUI.copyToClipboard(text); }
export function focusElement(elementId) { return window.ShellUI.focusElement(elementId); }
export function registerShortcut(handle, key, ctrl, meta, shift, alt, dotNetRef) {
    return window.ShellUI.registerShortcut(handle, key, ctrl, meta, shift, alt, dotNetRef);
}
export function unregisterShortcut(handle) { return window.ShellUI.unregisterShortcut(handle); }
