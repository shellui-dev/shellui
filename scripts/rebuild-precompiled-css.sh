#!/usr/bin/env bash
# Regenerates src/ShellUI.Components/wwwroot/shellui-all.css — the pre-compiled
# Tailwind bundle NuGet consumers link to when they don't want a Tailwind build
# of their own. Downloads the Tailwind standalone CLI on first run and caches it
# at ~/.shellui/bin/. CI runs this via `.github/workflows/*.yml` and validates no
# drift with `git diff --exit-code`.
#
# Usage: ./scripts/rebuild-precompiled-css.sh
set -euo pipefail

# Version comes from TailwindConstants.cs — single source of truth. Portable awk
# instead of grep -P because Perl regex isn't universally available on CI runners.
TAILWIND_VERSION="$(awk -F'"' '/public const string Version/ {print $2; exit}' src/ShellUI.Core/TailwindConstants.cs)"
if [[ -z "$TAILWIND_VERSION" ]]; then
  echo "Could not extract Tailwind version from src/ShellUI.Core/TailwindConstants.cs" >&2
  exit 1
fi
echo "Using Tailwind CSS v$TAILWIND_VERSION"

# Detect platform for the standalone binary name.
case "$(uname -sm)" in
  "Linux x86_64")   PLATFORM="linux-x64" ;;
  "Linux aarch64")  PLATFORM="linux-arm64" ;;
  "Darwin x86_64")  PLATFORM="macos-x64" ;;
  "Darwin arm64")   PLATFORM="macos-arm64" ;;
  MINGW*|CYGWIN*|MSYS*) PLATFORM="windows-x64.exe" ;;
  *) echo "Unsupported platform: $(uname -sm)" >&2; exit 1 ;;
esac

# Cache in ~/.shellui/bin so this doesn't re-download per project or per CI run.
CACHE_DIR="${SHELLUI_CACHE_DIR:-$HOME/.shellui/bin}"
BINARY_NAME="tailwindcss-$TAILWIND_VERSION-$PLATFORM"
BINARY_PATH="$CACHE_DIR/$BINARY_NAME"

if [[ ! -f "$BINARY_PATH" ]]; then
  mkdir -p "$CACHE_DIR"
  URL="https://github.com/tailwindlabs/tailwindcss/releases/download/v$TAILWIND_VERSION/tailwindcss-$PLATFORM"
  echo "Downloading $URL"
  curl -fsSL "$URL" -o "$BINARY_PATH"
  chmod +x "$BINARY_PATH"
fi

INPUT_CSS="$(mktemp --suffix=.css)"
COMPONENTS_ROOT="src/ShellUI.Components"

# Compose the input fixture: theme block + inline safelist.
# We use `@source inline("…")` instead of `@source "file.txt"` because Tailwind's
# default file extractor fails to pick up classes with `[state=…]` arbitrary
# values from plain text files. Inline is explicit — each entry becomes a rule
# regardless of shape.
cat "$COMPONENTS_ROOT/wwwroot/shellui-theme.css" > "$INPUT_CSS"
echo "" >> "$INPUT_CSS"

# Feed classes space-separated inside a single @source inline("…") call. Split
# into ~500-char chunks so we don't hit any parser limits on very long strings.
awk '
  BEGIN { line=""; }
  {
    if (length(line) + length($0) + 1 > 500) {
      printf "@source inline(\"%s\");\n", line;
      line=$0;
    } else if (line=="") {
      line=$0;
    } else {
      line=line " " $0;
    }
  }
  END { if (line != "") printf "@source inline(\"%s\");\n", line; }
' "$COMPONENTS_ROOT/wwwroot/shellui-classes.txt" >> "$INPUT_CSS"

OUTPUT_CSS="$COMPONENTS_ROOT/wwwroot/shellui-all.css"

echo "Compiling → $OUTPUT_CSS"
"$BINARY_PATH" -i "$INPUT_CSS" -o "$OUTPUT_CSS" --minify

SIZE=$(wc -c < "$OUTPUT_CSS")
echo "Wrote $(printf '%s' "$SIZE") bytes"
rm -f "$INPUT_CSS"
