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
            const hasModifier = (ctrl && e.ctrlKey) || (meta && e.metaKey);
            if ((ctrl || meta) ? !hasModifier : (e.ctrlKey || e.metaKey)) return;
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

    // Here rather than in shellui-sidebar.js: a dynamic import 404s once compiled into a consumer's RCL.
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