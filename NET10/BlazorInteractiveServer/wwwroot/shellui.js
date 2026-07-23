window.ShellUI = {
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
            // Create a new DataTransfer object
            const dt = new DataTransfer();
            
            // Get files from the drag event's DataTransfer
            let files;
            if (dataTransfer && dataTransfer.files) {
                files = dataTransfer.files;
            } else {
                return false;
            }
            
            // Add files to the new DataTransfer
            for (let i = 0; i < files.length; i++) {
                dt.items.add(files[i]);
            }
            
            // Assign the files from the DataTransfer to the input
            inputElement.files = dt.files;
            
            // Trigger change event
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
        
        if (!dropZone || !inputElement) {
            return false;
        }
        
        // Remove existing listener if any
        dropZone.removeEventListener('drop', handleDrop);
        
        function handleDrop(e) {
            e.preventDefault();
            e.stopPropagation();
            
            const dataTransfer = e.dataTransfer;
            if (!dataTransfer || !dataTransfer.files || dataTransfer.files.length === 0) {
                return;
            }
            
            try {
                // Create a new DataTransfer object
                const dt = new DataTransfer();
                
                // Add files from the drag event to the new DataTransfer
                const files = dataTransfer.files;
                for (let i = 0; i < files.length; i++) {
                    dt.items.add(files[i]);
                }
                
                // Assign the files from the DataTransfer to the input
                inputElement.files = dt.files;
                
                // Trigger change event
                const changeEvent = new Event('change', { bubbles: true });
                inputElement.dispatchEvent(changeEvent);
            } catch (error) {
                console.error('Error handling drop files:', error);
            }
        }
        
        // Attach the drop handler
        dropZone.addEventListener('drop', handleDrop);
        
        return true;
    },
    
    openUrl: function (url, target) {
        if (window.open) {
            window.open(url, target || '_blank');
        } else {
            console.error('window.open is not available');
        }
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

    // Fire a .NET callback when the window scrolls or resizes. Used by dropdowns
    // to close when the page scrolls (Blazor can't reposition popovers natively).
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
};

