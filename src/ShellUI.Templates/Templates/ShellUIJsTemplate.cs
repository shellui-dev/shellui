using ShellUI.Core.Models;

namespace ShellUI.Templates.Templates;

public static class ShellUIJsTemplate
{
    public static ComponentMetadata Metadata => new()
    {
        Name = "shellui-js",
        DisplayName = "ShellUI JS",
        Description = "JavaScript utilities for CopyButton, FileUpload, Command (clipboard, drag-drop, openUrl)",
        Category = ComponentCategory.Utility,
        FilePath = "../../wwwroot/shellui.js",
        IsAvailable = false
    };

    public static string Content => """
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
    }
};
""";
}
