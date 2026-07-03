/**
 * ShellUI utility functions for Blazor components
 */

export function copyToClipboard(text) {
  return navigator.clipboard.writeText(text);
}

// Global keyboard-shortcut registry keyed by handle so Blazor can dispose cleanly.
const shortcutHandlers = new Map();

export function registerShortcut(handle, key, ctrl, meta, shift, alt, dotNetRef) {
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
}

export function unregisterShortcut(handle) {
  const listener = shortcutHandlers.get(handle);
  if (listener) {
    window.removeEventListener("keydown", listener);
    shortcutHandlers.delete(handle);
  }
}
