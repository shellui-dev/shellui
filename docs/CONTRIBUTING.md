# Contributing to ShellUI

Thank you for your interest in contributing to ShellUI! This document provides guidelines and information for contributors.

## Current Status

ShellUI is currently in active development working towards v1.0. The project is not yet accepting external contributions, but we welcome feedback and suggestions through GitHub Issues.

### When Can I Contribute?

We will open up contributions after:
- Alpha release (CLI + Core Components)
- Contribution guidelines are finalized
- Component architecture is stabilized

For now, you can:
- Star the repository to show support
- Watch for updates
- Open issues for bug reports or feature suggestions (will be labeled for future consideration)
- Join discussions about the project direction

## Development Roadmap

See [MILESTONES.md](MILESTONES.md) for the detailed development roadmap and current progress.

## Future Contribution Areas

Once we open up contributions, we'll need help with:

### 1. Components
- New component development
- Component enhancements
- Bug fixes
- Accessibility improvements
- Test coverage

### 2. Documentation
- Component documentation
- Usage examples
- Tutorials and guides
- API documentation
- Video tutorials

### 3. CLI Tool
- New commands
- Command enhancements
- Cross-platform testing
- Bug fixes

### 4. Testing
- Unit tests
- Integration tests
- E2E tests
- Accessibility tests
- Browser compatibility tests

### 5. Design
- Component designs
- Documentation website design
- Marketing materials
- Demo applications

## Code Style Guidelines (Future)

When contributions are accepted, we'll follow these guidelines:

### C# Code Style
- Follow Microsoft's C# coding conventions
- Use meaningful variable and method names
- Add XML documentation comments for public APIs
- Keep methods focused and concise
- Use LINQ where appropriate

### Razor Component Style
```razor
@* Component documentation *@
<div class="@CssClass">
    @ChildContent
</div>

@code {
    [Parameter] public string Variant { get; set; } = "default";
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    
    private string CssClass => BuildCssClass();
    
    private string BuildCssClass()
    {
        // Class building logic
        return "base-classes variant-classes";
    }
}
```

### Tailwind CSS Guidelines
- Use Tailwind utility classes (no custom CSS)
- Follow mobile-first responsive design
- Use design tokens from theme
- Maintain dark mode compatibility
- Keep specificity low

### Testing Guidelines
- Write unit tests for all components
- Include accessibility tests
- Test keyboard navigation
- Test screen reader compatibility
- Test on multiple browsers

## Git Workflow (Future)

When contributions are accepted:

1. Fork the repository
2. Create a feature branch
   - `feature/component-name` for new components
   - `fix/issue-description` for bug fixes
   - `docs/what-changed` for documentation
3. Make your changes
4. Write/update tests
5. Ensure all tests pass
6. Update documentation
7. Submit a pull request

### Commit Message Convention
```
type(scope): brief description

Detailed description if needed

Fixes #issue-number
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`

Examples:
```
feat(button): add loading state
fix(input): resolve validation styling issue
docs(readme): update installation instructions
```

## Component Development Guidelines (Future)

When developing components:

### Component Structure
```
ComponentName/
├── ComponentName.razor          # Main component
├── ComponentName.razor.cs       # Code-behind (if needed)
├── ComponentName.tests.cs       # Unit tests
├── metadata.json                # Component metadata
├── README.md                    # Component documentation
└── examples/
    ├── basic.razor
    ├── variants.razor
    └── composition.razor
```

### Component Checklist
- [ ] Implements common parameters (Variant, Size, Disabled, etc.)
- [ ] Supports `AdditionalAttributes` for extensibility
- [ ] Fully accessible (ARIA attributes, keyboard nav)
- [ ] Works in dark mode
- [ ] Responsive design
- [ ] Has comprehensive tests
- [ ] Has documentation
- [ ] Has usage examples
- [ ] Follows naming conventions
- [ ] Uses Tailwind utilities only

### Accessibility Requirements
Every component must:
- Be keyboard accessible
- Have proper ARIA attributes
- Have focus indicators
- Work with screen readers
- Have proper color contrast
- Support reduced motion preferences
- Be tested with accessibility tools

## Documentation Guidelines (Future)

When writing documentation:

### Component Documentation
Each component needs:
1. Description - What is it?
2. When to use - Use cases
3. API reference - All parameters, events, methods
4. Examples - Multiple usage examples
5. Accessibility notes - How it's accessible
6. Customization guide - How to customize it
7. Composition examples - Using with other components

### Code Examples
- Should be complete and runnable
- Should demonstrate best practices
- Should include comments for clarity
- Should show common use cases

## Testing Guidelines (Future)

### Unit Tests
```csharp
[Fact]
public void Button_RendersWithCorrectVariant()
{
    // Arrange
    var cut = RenderComponent<Button>(parameters => parameters
        .Add(p => p.Variant, "outline")
        .Add(p => p.ChildContent, "Click Me"));
    
    // Act
    var button = cut.Find("button");
    
    // Assert
    button.ClassList.Should().Contain("button-outline");
    button.TextContent.Should().Be("Click Me");
}
```

### Accessibility Tests
```csharp
[Fact]
public void Button_HasProperAriaAttributes()
{
    var cut = RenderComponent<Button>(parameters => parameters
        .Add(p => p.Disabled, true));
    
    var button = cut.Find("button");
    button.GetAttribute("aria-disabled").Should().Be("true");
}
```

## Building and Running Locally (Future)

### Prerequisites
- .NET 8.0 SDK or higher
- Node.js 18+ (for Tailwind CSS)
- Git

### Setup
```bash
# Clone your fork
git clone https://github.com/shellui-dev/shellui.git
cd shellui

# Restore packages
dotnet restore

# Install npm dependencies
npm install

# Build the solution
dotnet build

# Run tests
dotnet test

# Run example project
cd examples/BlazorServer
dotnet run
```

## Questions?

For now, open an issue with your question and we'll respond as soon as possible.

Once we open contributions, we'll set up:
- Discord server for real-time chat
- GitHub Discussions for longer conversations
- Regular contributor calls

## Code of Conduct

We are committed to providing a welcoming and inclusive environment. We will publish a full Code of Conduct before accepting contributions.

Expected behavior:
- Be respectful and considerate
- Welcome newcomers
- Focus on constructive feedback
- Respect differing opinions
- Prioritize community safety

Unacceptable behavior:
- Harassment of any kind
- Discriminatory language or actions
- Personal attacks
- Trolling or inflammatory comments
- Any form of abuse

## License

By contributing to ShellUI, you agree that your contributions will be licensed under the MIT License.

---

## Stay Updated

- Star the repository
- Watch for release announcements
- Check [MILESTONES.md](MILESTONES.md) for progress
- Follow discussions

Thank you for your interest in ShellUI!

