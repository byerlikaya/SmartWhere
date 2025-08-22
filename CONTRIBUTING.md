# Contributing to SmartWhere

Thank you for your interest in contributing to SmartWhere! This document provides guidelines and information for contributors.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Setup](#development-setup)
- [Coding Standards](#coding-standards)
- [Testing](#testing)
- [Pull Request Process](#pull-request-process)
- [Release Process](#release-process)

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

## How Can I Contribute?

### Reporting Bugs

- Use the [GitHub issue tracker](https://github.com/byerlikaya/SmartWhere/issues)
- Include a clear and descriptive title
- Describe the exact steps to reproduce the problem
- Provide specific examples to demonstrate the steps
- Describe the behavior you observed after following the steps
- Explain which behavior you expected to see instead and why
- Include details about your configuration and environment

### Suggesting Enhancements

- Use the [GitHub issue tracker](https://github.com/byerlikaya/SmartWhere/issues)
- Provide a clear and descriptive title
- Describe the current behavior and explain which behavior you expected to see instead
- Explain why this enhancement would be useful to most SmartWhere users

### Pull Requests

- Fork the repository
- Create a feature branch (`git checkout -b feature/amazing-feature`)
- Make your changes
- Add tests for new functionality
- Ensure all tests pass
- Commit your changes (`git commit -m 'Add amazing feature'`)
- Push to the branch (`git push origin feature/amazing-feature`)
- Open a Pull Request

## Development Setup

### Prerequisites

- .NET 9.0 SDK or later
- Visual Studio 2022, VS Code, or Rider
- Git

### Getting Started

1. **Fork and Clone**
   ```bash
   git clone https://github.com/YOUR_USERNAME/SmartWhere.git
   cd SmartWhere
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the Project**
   ```bash
   dotnet build
   ```

4. **Run Tests**
   ```bash
   dotnet test
   ```

## Coding Standards

### C# Coding Standards

- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Keep methods small and focused
- Use SOLID principles
- Follow DRY (Don't Repeat Yourself) principle

### Code Style

- Use 4 spaces for indentation
- Use PascalCase for public members
- Use camelCase for private members and parameters
- Use meaningful names that describe the purpose
- Add comments for complex logic

### Example

```csharp
/// <summary>
/// Filters an IQueryable collection based on WhereClause attributes.
/// </summary>
/// <typeparam name="T">The type of entities to filter.</typeparam>
/// <param name="query">The IQueryable collection to filter.</param>
/// <param name="filterObject">The object containing filter values.</param>
/// <returns>A filtered IQueryable collection.</returns>
public static IQueryable<T> Where<T>(this IQueryable<T> query, object filterObject)
{
    // Implementation
}
```

## Testing

### Test Requirements

- All new features must include unit tests
- All bug fixes must include regression tests
- Maintain at least 80% code coverage
- Tests should be fast, reliable, and isolated

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/SmartWhere.Tests/
```

### Test Naming Convention

```csharp
[Test]
public void MethodName_Scenario_ExpectedBehavior()
{
    // Arrange
    // Act
    // Assert
}
```

## Pull Request Process

### Before Submitting

1. **Ensure your code compiles** without errors or warnings
2. **Run all tests** and ensure they pass
3. **Update documentation** if needed
4. **Check code style** and formatting
5. **Verify functionality** works as expected

### Pull Request Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## Testing
- [ ] Unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing completed

## Checklist
- [ ] My code follows the style guidelines of this project
- [ ] I have performed a self-review of my own code
- [ ] I have commented my code, particularly in hard-to-understand areas
- [ ] I have made corresponding changes to the documentation
- [ ] My changes generate no new warnings
- [ ] I have added tests that prove my fix is effective or that my feature works
- [ ] New and existing unit tests pass locally with my changes
```

## Release Process

### For Contributors

1. **Create a feature branch** from `master`
2. **Implement your changes** following coding standards
3. **Add tests** for new functionality
4. **Update documentation** if needed
5. **Submit a pull request** with detailed description
6. **Wait for review** and address feedback
7. **Once approved**, maintainers will merge and release

### Release Schedule

- **Patch releases** (2.2.x): Bug fixes and minor improvements
- **Minor releases** (2.x.0): New features and enhancements
- **Major releases** (x.0.0): Breaking changes and major features

## Getting Help

- **GitHub Issues**: [Create an issue](https://github.com/byerlikaya/SmartWhere/issues)
- **Discussions**: [Join discussions](https://github.com/byerlikaya/SmartWhere/discussions)
- **Wiki**: [Check documentation](https://github.com/byerlikaya/SmartWhere/wiki)

## Recognition

Contributors will be recognized in:
- Project README
- Release notes
- GitHub contributors list

## License

By contributing to SmartWhere, you agree that your contributions will be licensed under the MIT License.

---

Thank you for contributing to SmartWhere! 🚀
