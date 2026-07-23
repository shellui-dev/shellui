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
    },

    // Ref-counted body-scroll lock so nested modals (dialog opens a drawer opens a
    // sheet) don't prematurely unlock when the innermost closes.
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

    // Fire a .NET callback when the window scrolls or resizes. Used by dropdowns
    // to close when the page scrolls (native Blazor can't reposition popovers).
    // Bubble-phase listener means inner scrolling inside the dropdown itself
    // (e.g. scrolling through options) does NOT fire this — only page scroll does.
    // Fire a .NET callback when the page (window) scrolls or the window resizes.
    // Bubble-phase intentionally — scrolling inside an inner element (e.g. an option
    // list with overflow:auto) does NOT fire this, only page-level scroll does.
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
    }
});

// ES module re-exports so consumers who import this file dynamically still work.
export function copyToClipboard(text) { return window.ShellUI.copyToClipboard(text); }
export function focusElement(elementId) { return window.ShellUI.focusElement(elementId); }
export function registerShortcut(handle, key, ctrl, meta, shift, alt, dotNetRef) {
    return window.ShellUI.registerShortcut(handle, key, ctrl, meta, shift, alt, dotNetRef);
}
export function unregisterShortcut(handle) { return window.ShellUI.unregisterShortcut(handle); }
export function lockBodyScroll() { return window.ShellUI.lockBodyScroll(); }
export function unlockBodyScroll() { return window.ShellUI.unlockBodyScroll(); }
export function onDismissEvents(handle, dotNetRef) { return window.ShellUI.onDismissEvents(handle, dotNetRef); }
export function offDismissEvents(handle) { return window.ShellUI.offDismissEvents(handle); }
