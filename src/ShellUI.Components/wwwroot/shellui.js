/**
 * ShellUI utility functions for Blazor components
 */

export function copyToClipboard(text) {
  return navigator.clipboard.writeText(text);
}
