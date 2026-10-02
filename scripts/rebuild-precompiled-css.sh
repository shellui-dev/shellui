#!/usr/bin/env bash
# Builds the precompiled bundle NuGet consumers can link instead of running Tailwind.
# Usage: ./scripts/rebuild-precompiled-css.sh
set -euo pipefail

# Version from TailwindConstants.cs; awk because grep -P isn't on every runner.
TAILWIND_VERSION="$(awk -F'"' '/public const string Version/ {print $2; exit}' src/ShellUI.Core/TailwindConstants.cs)"
if [[ -z "$TAILWIND_VERSION" ]]; then
  echo "Could not extract Tailwind version from src/ShellUI.Core/TailwindConstants.cs" >&2
  exit 1
fi
echo "Using Tailwind CSS v$TAILWIND_VERSION"

case "$(uname -sm)" in
  "Linux x86_64")   PLATFORM="linux-x64" ;;
  "Linux aarch64")  PLATFORM="linux-arm64" ;;
  "Darwin x86_64")  PLATFORM="macos-x64" ;;
  "Darwin arm64")   PLATFORM="macos-arm64" ;;
  MINGW*|CYGWIN*|MSYS*) PLATFORM="windows-x64.exe" ;;
  *) echo "Unsupported platform: $(uname -sm)" >&2; exit 1 ;;
esac

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

# mktemp --suffix is GNU-only.
INPUT_CSS="$(mktemp "${TMPDIR:-/tmp}/shellui-input.XXXXXX")"
mv "$INPUT_CSS" "$INPUT_CSS.css"
INPUT_CSS="$INPUT_CSS.css"
COMPONENTS_ROOT="src/ShellUI.Components"

# @source inline, because Tailwind's file extractor misses `[state=…]` classes in plain text.
cat "$COMPONENTS_ROOT/wwwroot/shellui-theme.css" > "$INPUT_CSS"
echo "" >> "$INPUT_CSS"

# Chunks of ~500 chars to avoid very long strings.
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
