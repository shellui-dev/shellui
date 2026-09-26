#!/usr/bin/env bash
# Prints the section of docs/RELEASE_NOTES.md for one version, used as the GitHub
# release body. A section runs from its "# ShellUI v<version>" heading to the next
# release heading. "# ..." lines inside fenced code blocks (shell comments in install
# snippets) are not headings. Trailing blank lines and "---" separators are dropped.
#
# Exits 1 when the version has no section, so a tag can't publish without notes.
#
# Usage: scripts/extract-release-notes.sh <version> [notes-file]
#   e.g. scripts/extract-release-notes.sh 0.3.0-rc.2
set -euo pipefail

version="${1:?usage: extract-release-notes.sh <version> [notes-file]}"
file="${2:-docs/RELEASE_NOTES.md}"

if ! awk -v ver="v$version" '
  { line = $0; sub(/\r$/, "", line) }
  line ~ /^```/ { fence = !fence }
  !fence && line ~ /^# ShellUI v/ {
    if (found) exit
    split(line, parts, " ")
    if (parts[3] == ver) found = 1
  }
  found { buf[++n] = $0 }
  END {
    if (!found) exit 1
    while (n > 0) {
      last = buf[n]; sub(/\r$/, "", last)
      if (last ~ /^[[:space:]]*(---)?[[:space:]]*$/) n--; else break
    }
    for (i = 1; i <= n; i++) print buf[i]
  }
' "$file"; then
  echo "No '# ShellUI v$version' section in $file — add release notes before tagging." >&2
  exit 1
fi
