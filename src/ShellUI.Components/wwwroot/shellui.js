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

    addClassToDocument: function (className) {
        document.documentElement.classList.add(className);
    },

    removeClassFromDocument: function (className) {
        document.documentElement.classList.remove(className);
    },

    _themeObservers: new Map(),
    observeTheme: function (handle, dotNetRef) {
        this.unobserveTheme(handle);
        const root = document.documentElement;
        const observer = new MutationObserver(() => {
            dotNetRef.invokeMethodAsync("OnThemeChanged", root.classList.contains("dark")).catch(() => {});
        });
        observer.observe(root, { attributes: true, attributeFilter: ["class"] });
        this._themeObservers.set(handle, observer);
        return root.classList.contains("dark");
    },
    unobserveTheme: function (handle) {
        const observer = this._themeObservers.get(handle);
        if (observer) {
            observer.disconnect();
            this._themeObservers.delete(handle);
        }
    },

    setupFileDrop: function (dropZoneId, inputElementId) {
        const dropZone = document.getElementById(dropZoneId);
        const input = document.getElementById(inputElementId);
        if (!dropZone || !input) return false;
        if (dropZone._shelluiDrop) dropZone.removeEventListener("drop", dropZone._shelluiDrop);
        dropZone._shelluiDrop = (e) => {
            e.preventDefault();
            e.stopPropagation();
            const files = e.dataTransfer && e.dataTransfer.files;
            if (!files || files.length === 0) return;
            const dt = new DataTransfer();
            for (let i = 0; i < files.length; i++) dt.items.add(files[i]);
            input.files = dt.files;
            input.dispatchEvent(new Event("change", { bubbles: true }));
        };
        dropZone.addEventListener("drop", dropZone._shelluiDrop);
        return true;
    },

    registerShortcut: function (handle, key, ctrl, meta, shift, alt, dotNetRef) {
        const listener = (e) => {
            if (e.key.toLowerCase() !== key.toLowerCase()) return;
            const hasModifier = (ctrl && e.ctrlKey) || (meta && e.metaKey);
            if ((ctrl || meta) ? !hasModifier : (e.ctrlKey || e.metaKey)) return;
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

    // Ref-counted so nested overlays don't unlock early.
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

    // A stack, so a dialog opened from a dialog traps Tab inside the top one.
    _focusTraps: [],
    _focusableSelector: 'a[href], button:not([disabled]), input:not([disabled]):not([type="hidden"]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"]), [contenteditable="true"]',
    trapFocus: function (handle, element) {
        const shell = window.ShellUI;
        if (!element || shell._focusTraps.some(t => t.handle === handle)) return;
        if (shell._focusTraps.length === 0) document.addEventListener("keydown", shell._onFocusTrapKeyDown, true);
        shell._focusTraps.push({ handle: handle, element: element, previous: document.activeElement });
    },
    releaseFocus: function (handle) {
        const shell = window.ShellUI;
        const index = shell._focusTraps.findIndex(t => t.handle === handle);
        if (index < 0) return;
        const trap = shell._focusTraps.splice(index, 1)[0];
        if (shell._focusTraps.length === 0) document.removeEventListener("keydown", shell._onFocusTrapKeyDown, true);
        if (trap.previous && trap.previous.isConnected && typeof trap.previous.focus === "function") trap.previous.focus();
    },
    _onFocusTrapKeyDown: function (e) {
        if (e.key !== "Tab") return;
        const shell = window.ShellUI;
        const trap = shell._focusTraps[shell._focusTraps.length - 1];
        if (!trap || !trap.element.isConnected) return;
        const root = trap.element;
        const items = Array.from(root.querySelectorAll(shell._focusableSelector)).filter(el => el.getClientRects().length > 0);
        const active = document.activeElement;
        if (items.length === 0) {
            e.preventDefault();
            root.focus();
            return;
        }
        const first = items[0];
        const last = items[items.length - 1];
        if (!root.contains(active)) {
            e.preventDefault();
            (e.shiftKey ? last : first).focus();
        } else if (e.shiftKey && (active === first || active === root)) {
            e.preventDefault();
            last.focus();
        } else if (!e.shiftKey && active === last) {
            e.preventDefault();
            first.focus();
        }
    },

    // Bubble phase: scrolling inside the dropdown doesn't fire, only page scroll does.
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
export function addClassToDocument(className) { return window.ShellUI.addClassToDocument(className); }
export function removeClassFromDocument(className) { return window.ShellUI.removeClassFromDocument(className); }
export function observeTheme(handle, dotNetRef) { return window.ShellUI.observeTheme(handle, dotNetRef); }
export function unobserveTheme(handle) { return window.ShellUI.unobserveTheme(handle); }
export function setupFileDrop(dropZoneId, inputElementId) { return window.ShellUI.setupFileDrop(dropZoneId, inputElementId); }
export function registerShortcut(handle, key, ctrl, meta, shift, alt, dotNetRef) {
    return window.ShellUI.registerShortcut(handle, key, ctrl, meta, shift, alt, dotNetRef);
}
export function unregisterShortcut(handle) { return window.ShellUI.unregisterShortcut(handle); }
export function lockBodyScroll() { return window.ShellUI.lockBodyScroll(); }
export function unlockBodyScroll() { return window.ShellUI.unlockBodyScroll(); }
export function trapFocus(handle, element) { return window.ShellUI.trapFocus(handle, element); }
export function releaseFocus(handle) { return window.ShellUI.releaseFocus(handle); }
export function onDismissEvents(handle, dotNetRef) { return window.ShellUI.onDismissEvents(handle, dotNetRef); }
export function offDismissEvents(handle) { return window.ShellUI.offDismissEvents(handle); }
