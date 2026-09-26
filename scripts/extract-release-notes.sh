#!/usr/bin/env bash
# Usage: scripts/extract-release-notes.sh <version> [notes-file]
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
