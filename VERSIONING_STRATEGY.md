# ShellUI Versioning Strategy

## Current Version and Publication Boundary

ShellUI uses one centralized version for the source templates, CLI tool, and packable runtime components.

| Scope | Current fact |
|---|---|
| Current source | `0.4.0-alpha.1` |
| Base version in `Directory.Build.props` | `0.4.0` |
| Prerelease suffix | `alpha.1` |
| Current target framework | .NET 10 |
| Current Tailwind version | `4.3.2` |
| Latest tag present in this checkout | `v0.3.0-rc.1` (historical) |
| Published package status | Not implied by the current source version; verify the package channel and pin an explicit version |

`0.4.0-alpha.1` describes this checkout. It must not be described as a published NuGet or GitHub release unless a matching release has actually been published. The historical `v0.3.0-rc.1` tag is retained as release history, not as the current source version. The published prerelease targets .NET 9; the current source targets .NET 10.

## Single Source of Truth

The root `Directory.Build.props` supplies the version:

```xml
<PropertyGroup>
  <ShellUIVersion>0.4.0</ShellUIVersion>
  <ShellUIVersionSuffix>alpha.1</ShellUIVersionSuffix>
</PropertyGroup>
```

`Directory.Build.props` composes the package and assembly metadata:

- `Version` becomes `0.4.0-alpha.1` when a suffix is present.
- `AssemblyVersion` and `FileVersion` use the numeric base `0.4.0`.
- `InformationalVersion` includes the prerelease suffix.
- Component metadata reads the centralized properties when running from the repository. `VersionHelper` uses assembly metadata as a fallback when its repository search does not find the solution file.

This means a release version is changed in one file, not independently in every template or package project.

## What Receives the Version

The version is used consistently by:

- `ShellUI.CLI`, which is packed as a .NET global tool.
- `ShellUI.Components`, which is packed as an independent Razor class library.
- CLI source templates and component metadata.
- The internal Core and Templates projects during the repository build.
- The test and safelist utility projects during the repository build.

Only `ShellUI.CLI` and `ShellUI.Components` are packable. `ShellUI.Core` and `ShellUI.Templates` are internal projects with `IsPackable=false`; they are not consumer packages. The test project and `ShellUI.SafelistGenerator` are also non-packable.

## Installed Source Version

The CLI writes the computed version into `shellui.json` for each installed entry. The relevant shape is:

```json
{
  "Schema": "https://shellui.dev/schema.json",
  "Style": "default",
  "ComponentsPath": "Components/UI",
  "LayoutPath": "Components/Layout",
  "InstalledComponents": [
    {
      "Name": "button",
      "Version": "0.4.0-alpha.1",
      "InstalledAt": "2026-01-01T00:00:00Z",
      "IsCustomized": false
    }
  ],
  "ProjectType": 1
}
```

The timestamp is illustrative. A copied component is source owned by the consumer, so its recorded version identifies the registry release used at install or update time; it does not automatically track a later package publication.

Theme data has a separate lock file. `shellui.theme.lock` records the theme source URL, content hash, theme name, and application time. That lock controls `theme update` and is independent of the component version.

## User Version Selection

### Published CLI

Use an explicit version when reproducibility matters:

```bash
dotnet tool install -g ShellUI.CLI --version <published-version>
dotnet tool update -g ShellUI.CLI --version <published-version>
```

A local tool manifest can pin the same version for a team. The current source version is not automatically selected by an unversioned global install.

### Published Components package

`ShellUI.Components` is the runtime RCL package. Select its published version explicitly when using NuGet:

```bash
dotnet add package ShellUI.Components --version <published-version> --prerelease
```

Do not add `ShellUI.Core` or `ShellUI.Templates` to a consumer project. They are internal implementation projects in the current source.

### Current source checkout

To test the checkout itself, pack the solution and install the locally produced CLI package:

```bash
dotnet pack ShellUI.slnx --configuration Release
dotnet tool install -g ShellUI.CLI --add-source "./src/ShellUI.CLI/bin/Release" --version 0.4.0-alpha.1
```

This local package is a build artifact, not evidence of publication to NuGet.

### Source tags and documentation

Release tags use the `v` prefix, for example `v0.4.0-alpha.1`. A tag is a source-control reference; publication is performed separately by the release workflow. Current documentation describes the checkout, while [RELEASE_NOTES.md](docs/RELEASE_NOTES.md) preserves historical release records. There are no versioned documentation directories in this repository to maintain.

## Version Update Workflow

For a maintainer changing the version:

1. Edit `ShellUIVersion` and `ShellUIVersionSuffix` in `Directory.Build.props`.
2. Run the repository build and tests against `ShellUI.slnx`.
3. Regenerate the precompiled CSS and safelist when the change affects component CSS.
4. Pack the solution and verify that only the CLI and Components packages are produced.
5. Create a matching `v<version>` tag and let the release workflow publish the packable artifacts.

The verification commands are:

```bash
dotnet restore ShellUI.slnx
dotnet run --project tools/ShellUI.SafelistGenerator -- src/ShellUI.Components/Components src/ShellUI.Components/wwwroot/shellui-classes.txt src/ShellUI.Components/build/ShellUI.Components.targets
bash scripts/rebuild-precompiled-css.sh
dotnet build ShellUI.slnx --configuration Release
dotnet test ShellUI.slnx --no-restore --no-build --configuration Release --verbosity normal
dotnet pack ShellUI.slnx --no-build --configuration Release
```

The safelist generator updates the committed class list and package targets; `scripts/rebuild-precompiled-css.sh` consumes that list to build the CSS bundle and does not regenerate the safelist. The demo is not part of the solution build and is run separately:

```bash
dotnet run --project NET10/BlazorInteractiveServer/BlazorInteractiveServer.csproj
```

The release workflow builds and tests the six-project solution, packs it, publishes `ShellUI.CLI` and `ShellUI.Components`, and creates a GitHub release for a pushed `v*` tag. A local build or tag without the publication workflow is not a release.

## Semantic Versioning

ShellUI versions use:

```text
MAJOR.MINOR.PATCH[-PRERELEASE]
```

Examples in the current version line are `0.4.0-alpha.1` and a numeric `0.4.0` value only if the suffix is removed for a stable release. The suffix communicates prerelease status; it is not a component-specific version.

- Increase the major version for a deliberate breaking version boundary.
- Increase the minor version for compatible feature work within the current version line.
- Increase the patch version for fixes without an intended contract change.
- Use an empty suffix for a stable release only after the release decision is made.

The unified version applies to the source system and the two packable artifacts. There is currently no independent version stream for individual components.

## Compatibility Boundaries

- The current repository baseline is .NET 10. This strategy does not claim support for an older target framework that is not configured in the current projects.
- Tailwind CSS `4.3.2` is the current generated-CSS baseline.
- The CLI and the NuGet RCL are separate consumption paths, but a project that combines them should use versions from the same ShellUI release line.
- CLI-installed files are copied into the consumer project and remain subject to the consumer's own edits. Updating them is an explicit CLI operation, not an automatic package update.
- Theme lock files track theme sources, not package versions.

## Historical Release Information

The [historical release notes](docs/RELEASE_NOTES.md) contain older release headings, fixes, and install examples. They are preserved as history and should be read with the version they describe. They do not override the current source facts in this document.

## Summary

The current source is `0.4.0-alpha.1`, centralized in `Directory.Build.props`, targeting .NET 10 and Tailwind CSS `4.3.2`. `ShellUI.CLI` and `ShellUI.Components` are the only packable artifacts. Published versions must be selected explicitly; the source version and the latest historical tag are not interchangeable.
