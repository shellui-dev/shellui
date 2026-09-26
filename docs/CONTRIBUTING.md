# Contributing to ShellUI

ShellUI targets .NET 10 and Tailwind CSS `4.3.2`. This guide describes the contribution and verification workflow; releases are covered in [VERSIONING_STRATEGY.md](../VERSIONING_STRATEGY.md).

## Contribution Policy

ShellUI is an active alpha project. Focused bug fixes, documentation corrections, tests, and component or CLI improvements are welcome through GitHub issues and pull requests.

Before starting a substantial new component, a new distribution path, or a change to the registry contract:

1. Search existing issues and pull requests.
2. Open an issue describing the problem, intended behavior, and affected projects.
3. Agree on the smallest useful scope before implementing a large change.
4. Keep the change compatible with the current architecture unless the proposal explicitly changes that architecture.

Alpha software can change its APIs and generated source. An issue or pull request is not a promise that a feature will be accepted, scheduled, or released. There is no response-time or adoption target in this policy.

Implemented features should be described as implemented. Ideas that have no implementation or release assignment belong in an issue or in the unscheduled section below, not in current-status documentation.

## Repository Scope

The solution file is `ShellUI.slnx`. It contains six projects:

- `src/ShellUI.CLI/ShellUI.CLI.csproj` — packable .NET global tool.
- `src/ShellUI.Components/ShellUI.Components.csproj` — packable independent Razor class library.
- `src/ShellUI.Core/ShellUI.Core.csproj` — internal models, configuration, and Tailwind constants.
- `src/ShellUI.Templates/ShellUI.Templates.csproj` — internal registry and source templates.
- `tools/ShellUI.SafelistGenerator/ShellUI.SafelistGenerator.csproj` — internal CSS safelist utility.
- `ShellUI.Tests/ShellUI.Tests.csproj` — test project.

The runnable demo at `NET10/BlazorInteractiveServer/BlazorInteractiveServer.csproj` is outside `ShellUI.slnx` and must be built or run explicitly. Only `ShellUI.CLI` and `ShellUI.Components` are packable. `ShellUI.Core` and `ShellUI.Templates` are internal implementation projects, not consumer packages.

## Prerequisites

- .NET 10 SDK. `global.json` requests SDK `10.0.100` with feature-band roll-forward.
- Git.
- Node.js and npm only when using the npm Tailwind method. The standalone method does not require Node.js.

## Local Setup

From the repository root:

```bash
dotnet restore ShellUI.slnx
dotnet build ShellUI.slnx --configuration Release
```

The CLI supports the implemented commands `init`, `add`, `list`, `remove`, `update`, `theme init`, `theme apply`, and `theme update`. Use a local build when testing changes; do not substitute an older published CLI merely because it is easier to install.

The precompiled CSS and safelist are generated as part of the CI workflow. When changing component CSS, variant helpers, or safelist inputs, regenerate the safelist and then build the bundle:

```bash
dotnet run --project tools/ShellUI.SafelistGenerator -- src/ShellUI.Components/Components src/ShellUI.Components/wwwroot/shellui-classes.txt src/ShellUI.Components/build/ShellUI.Components.targets
bash scripts/rebuild-precompiled-css.sh
```

The safelist generator updates `shellui-classes.txt` and the package targets; the CSS script consumes those files and does not regenerate the safelist.

## Tests and Verification

The normal local sequence is:

```bash
dotnet build ShellUI.slnx --configuration Release
dotnet test ShellUI.slnx --no-restore --no-build --configuration Release --verbosity normal
```

The current `ShellUI.Tests` project uses:

- xUnit with the Visual Studio runner.
- `Microsoft.NET.Test.Sdk`.
- Roslyn through `Microsoft.CodeAnalysis.CSharp`.
- `coverlet.collector`.

The current tests cover registry and dependency integrity, template parsing, template-to-RCL synchronization, initialization and host bootstrap, chart assets, safelist drift, and related CLI behavior. There is currently no bUnit, FluentAssertions, rendered-component, or browser-automation suite. Do not add examples or tests that assume those packages or a browser runner. If rendered UI coverage is needed, propose the harness and its dependencies as part of the change.

The CI workflow additionally packs the CLI, scaffolds a fresh Blazor application, runs `init` and `add`, builds the result, and verifies the NuGet-only package path. A local change should be considered ready when the solution build and test commands pass and any affected CLI smoke scenario has been checked.

To exercise the demo outside the solution:

```bash
dotnet run --project NET10/BlazorInteractiveServer/BlazorInteractiveServer.csproj
```

## Component Changes

Component work has two representations that must remain synchronized:

- The live Razor class library under `src/ShellUI.Components/Components/`.
- The CLI source templates and registry under `src/ShellUI.Templates/`.

For a component change:

1. Update the live component and its generated template together.
2. Update registry metadata, dependencies, `FilePath`, variants, and NuGet dependencies when the public contract changes.
3. Preserve the distinction between direct targets and hidden dependency entries. The current registry has 176 entries, including 76 direct targets and 100 hidden entries.
4. Use the Tailwind `4.3.2` variable and utility conventions already used by the project.
5. Add or update tests that exercise the changed registry, template, or CLI behavior.
6. Review keyboard and assistive-technology behavior without claiming a blanket accessibility conformance level that the repository does not test.

Do not edit the demo as a substitute for updating the source library or template. The demo is a consumer fixture outside the solution.

## CLI Changes

CLI changes should preserve the implemented command tree and its actual options. Document a command only when its handler and user-facing help are implemented in the same change; do not list speculative commands or options.

For a new command or option:

- Add its handler and user-facing help text.
- Add or update tests for success, failure, and relevant project-state behavior.
- Update the command reference and examples in the same pull request.
- Check that generated files, `shellui.json`, and host bootstrap behavior remain consistent.

## Documentation Changes

Documentation in this repository should be verifiable against the checkout:

- Refer to `Directory.Build.props` for the version instead of repeating it; install commands may pin the latest release.
- Use `ShellUI.slnx` and the six solution project paths; do not point contributors at paths that are not in the checkout.
- Distinguish the demo at `NET10/BlazorInteractiveServer` from solution membership.
- Use the actual CLI command names and current test packages.
- Link only to files that exist in the repository.
- Add a `# ShellUI v<version>` section to `RELEASE_NOTES.md` for each release; do not rewrite earlier sections.
- Label unimplemented ideas as unscheduled proposals. Do not add dates, adoption targets, performance promises, or unsupported accessibility compliance claims.

## Style and Review Expectations

- Follow the existing C# and Razor conventions in neighboring files.
- Keep public behavior covered by the narrowest useful test.
- Avoid introducing a new runtime or test dependency without explaining why the existing stack is insufficient.
- Keep generated component source readable so a consumer can edit and own it.
- Keep changes focused and describe known gaps in the pull request.

A useful pull request explains the problem, lists the projects and tests changed, records the commands run, and calls out any behavior that remains intentionally unaddressed.

## Unscheduled Proposals

The following are possible areas for discussion, not current features or commitments:

- Additional components beyond the current registry.
- A larger documentation site or visual preview experience.
- Browser, end-to-end, and accessibility-focused automated test infrastructure.
- Further performance, packaging, and migration work.

Propose these through the issue tracker so their status and scope are explicit. They should not be presented as shipped functionality or tied to an invented release date.

## Communication and License

Use issue and pull-request discussions to make technical feedback specific, respectful, and actionable. Contributions are made under the repository's MIT license terms.

## Related Documentation

- [Project overview](../README.md)
- [Architecture](ARCHITECTURE.md)
- [Project status](PROJECT_STATUS.md)
- [CLI syntax](CLI_SYNTAX.md)
- [CLI installation](CLI_INSTALLATION.md)
- [Component dependencies](COMPONENT_DEPENDENCIES.md)
- [Tailwind setup](tailwind-setup.md)
- [Versioning strategy](../VERSIONING_STRATEGY.md)
- [Historical release notes](RELEASE_NOTES.md)
- [GitHub issues](https://github.com/shellui-dev/shellui/issues)
