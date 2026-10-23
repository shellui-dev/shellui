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
    },

    // The browser moves the panels during a drag or key press and reports the sizes once, so it never waits on the server.
    _resizeHandles: new Map(),
    initResizeHandle: function (handle, element, dotNetRef) {
        const shell = window.ShellUI;
        shell.disposeResizeHandle(handle);
        if (!element) return;
        const context = () => {
            const group = element.parentElement;
            if (!group || element.dataset.disabled === "true") return null;
            const panels = Array.from(group.children).filter(c => c.hasAttribute("data-panel"));
            const index = Array.from(group.children).filter(c => c.hasAttribute("data-handle")).indexOf(element);
            const before = panels[index], after = panels[index + 1];
            if (!before || !after) return null;
            const read = (panel, key, fallback) => { const v = parseFloat(panel.dataset[key]); return isNaN(v) ? fallback : v; };
            const sizes = panels.map(p => read(p, "size", 0));
            const pair = sizes[index] + sizes[index + 1];
            return {
                vertical: group.dataset.direction === "vertical",
                panels, index, before, after, sizes, pair,
                min: Math.max(read(before, "minSize", 0), pair - read(after, "maxSize", 100)),
                max: Math.min(read(before, "maxSize", 100), pair - read(after, "minSize", 0))
            };
        };
        const apply = (ctx, size) => {
            size = Math.min(ctx.max, Math.max(ctx.min, size));
            ctx.sizes[ctx.index] = size;
            ctx.sizes[ctx.index + 1] = ctx.pair - size;
            ctx.before.style.flexGrow = ctx.sizes[ctx.index];
            ctx.after.style.flexGrow = ctx.sizes[ctx.index + 1];
            element.setAttribute("aria-valuenow", Math.round(size));
        };
        const commit = (ctx) => dotNetRef.invokeMethodAsync("OnLayoutChanged", ctx.sizes).catch(() => {});
        const onPointerDown = (e) => {
            if (e.button !== 0) return;
            const ctx = context();
            if (!ctx) return;
            const total = ctx.panels.reduce((sum, p) => sum + (ctx.vertical ? p.offsetHeight : p.offsetWidth), 0);
            if (!total) return;
            e.preventDefault();
            const start = ctx.vertical ? e.clientY : e.clientX;
            const startSize = ctx.sizes[ctx.index];
            element.setPointerCapture(e.pointerId);
            element.dataset.dragging = "true";
            document.body.style.cursor = ctx.vertical ? "row-resize" : "col-resize";
            document.body.style.userSelect = "none";
            const onMove = (m) => apply(ctx, startSize + ((ctx.vertical ? m.clientY : m.clientX) - start) / total * 100);
            const onUp = () => {
                element.removeEventListener("pointermove", onMove);
                element.removeEventListener("pointerup", onUp);
                element.removeEventListener("pointercancel", onUp);
                delete element.dataset.dragging;
                document.body.style.cursor = "";
                document.body.style.userSelect = "";
                commit(ctx);
            };
            element.addEventListener("pointermove", onMove);
            element.addEventListener("pointerup", onUp);
            element.addEventListener("pointercancel", onUp);
        };
        const onKeyDown = (e) => {
            const ctx = context();
            if (!ctx) return;
            const step = parseFloat(element.dataset.step) || 10;
            const back = ctx.vertical ? "ArrowUp" : "ArrowLeft";
            const forward = ctx.vertical ? "ArrowDown" : "ArrowRight";
            const current = ctx.sizes[ctx.index];
            let size;
            if (e.key === back) size = current - step;
            else if (e.key === forward) size = current + step;
            else if (e.key === "Home") size = ctx.min;
            else if (e.key === "End") size = ctx.max;
            else return;
            e.preventDefault();
            apply(ctx, size);
            commit(ctx);
        };
        element.addEventListener("pointerdown", onPointerDown);
        element.addEventListener("keydown", onKeyDown);
        shell._resizeHandles.set(handle, { element, onPointerDown, onKeyDown });
    },
    disposeResizeHandle: function (handle) {
        const shell = window.ShellUI;
        const h = shell._resizeHandles.get(handle);
        if (!h) return;
        h.element.removeEventListener("pointerdown", h.onPointerDown);
        h.element.removeEventListener("keydown", h.onKeyDown);
        shell._resizeHandles.delete(handle);
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
export function initResizeHandle(handle, element, dotNetRef) { return window.ShellUI.initResizeHandle(handle, element, dotNetRef); }
export function disposeResizeHandle(handle) { return window.ShellUI.disposeResizeHandle(handle); }
