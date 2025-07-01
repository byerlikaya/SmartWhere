# SmartWhere Documentation

## Table of Contents
1. [Overview](#overview)
2. [Installation](#installation)
3. [Quick Start](#quick-start)
4. [Core Concepts](#core-concepts)
5. [Attributes Reference](#attributes-reference)
6. [Usage Examples](#usage-examples)
7. [Advanced Features](#advanced-features)
8. [API Reference](#api-reference)
9. [Best Practices](#best-practices)
10. [Troubleshooting](#troubleshooting)
11. [Contributing](#contributing)
12. [License](#license)

## Overview

**SmartWhere** is a powerful .NET library that intelligently filters sequences of items using a simple and practical attribute-based approach. It extends the standard `Queryable.Where` method to provide smart filtering capabilities based on request objects decorated with attributes.

### Key Features

- **Attribute-based filtering**: Define filter criteria using simple attributes
- **Multiple comparison operators**: Support for equality, inequality, greater than, less than, etc.
- **String matching methods**: Contains, StartsWith, EndsWith with negative variants
- **Logical operators**: AND/OR logic for combining multiple filters
- **Deep property navigation**: Filter on nested object properties
- **Type-safe**: Compile-time safety with strongly-typed request objects
- **Entity Framework compatible**: Works seamlessly with EF Core and EF6
- **Minimal configuration**: Zero-configuration setup with sensible defaults

### Supported Frameworks

- .NET Standard 2.0
- .NET Standard 2.1
- Compatible with .NET Core, .NET 5+, and .NET Framework

## Installation

Install the SmartWhere NuGet package using one of the following methods:

### Package Manager Console
```powershell
PM> Install-Package SmartWhere
```

### .NET CLI
```bash
dotnet add package SmartWhere
```

### PackageReference
```xml
<PackageReference Include="SmartWhere" Version="2.2.2.1" />
```

## Quick Start

### 1. Create a Request Class

Define a request class that implements the `IWhereClause` interface and decorate properties with attributes:

```csharp
using SmartWhere.Attributes;
using SmartWhere.Interfaces;

public class PublisherSearchRequest : IWhereClause
{
    [WhereClause]
    public int? Id { get; set; }

    [WhereClause(PropertyName = "Name")]
    public string PublisherName { get; set; }

    [WhereClause("Books.Author.Name")]
    public string AuthorName { get; set; }
}
```

### 2. Use in Your Query

Apply the filter using the `Where` extension method:

```csharp
[HttpPost]
public IActionResult GetPublishers(PublisherSearchRequest request)
{
    var result = _context.Publishers
        .Include(x => x.Books)
        .ThenInclude(x => x.Author)
        .Where(request)  // SmartWhere extension
        .ToList();

    return Ok(result);
}
```

## Core Concepts

### IWhereClause Interface

All request classes must implement the `IWhereClause` interface:

```csharp
public interface IWhereClause
{
    // Marker interface - no implementation required
}
```

### Property Mapping

SmartWhere automatically maps request properties to entity properties using:

1. **Explicit mapping**: Using the `PropertyName` parameter in attributes
2. **Implicit mapping**: Using the same property name as the entity
3. **Navigation properties**: Using dot notation for nested properties

### Null Value Handling

SmartWhere automatically ignores properties with null values, ensuring only meaningful filter criteria are applied.

## Attributes Reference

### WhereClause Attribute

The basic attribute for simple equality filtering.

#### Syntax
```csharp
[WhereClause]
[WhereClause(PropertyName = "EntityPropertyName")]
[WhereClause(LogicalOperator = LogicalOperator.OR)]
[WhereClause("Navigation.Property", LogicalOperator.AND)]
```

#### Parameters
- **PropertyName** (optional): The target entity property name
- **LogicalOperator** (optional): AND or OR logic (default: AND)

#### Examples
```csharp
public class BasicSearchRequest : IWhereClause
{
    [WhereClause] // Maps to entity's Id property
    public int? Id { get; set; }

    [WhereClause(PropertyName = "Name")] // Maps to entity's Name property
    public string Title { get; set; }

    [WhereClause("Books.Author.Name")] // Navigation property
    public string AuthorName { get; set; }

    [WhereClause(LogicalOperator = LogicalOperator.OR)]
    public string Category { get; set; }
}
```

### ComparativeWhereClause Attribute

Enables comparison operations beyond simple equality.

#### Syntax
```csharp
[ComparativeWhereClause(ComparisonOperator.GreaterThan)]
[ComparativeWhereClause("PropertyName", ComparisonOperator.LessThanOrEqual)]
[ComparativeWhereClause(ComparisonOperator.NotEqual, LogicalOperator.OR)]
```

#### Comparison Operators
- `Equal`: Property equals value (default behavior)
- `NotEqual`: Property does not equal value
- `GreaterThan`: Property is greater than value
- `NotGreaterThan`: Property is not greater than value
- `GreaterThanOrEqual`: Property is greater than or equal to value
- `NotGreaterThanOrEqual`: Property is not greater than or equal to value
- `LessThan`: Property is less than value
- `NotLessThan`: Property is not less than value
- `LessThanOrEqual`: Property is less than or equal to value
- `NotLessThanOrEqual`: Property is not less than or equal to value

#### Examples
```csharp
public class ComparativeSearchRequest : IWhereClause
{
    [ComparativeWhereClause("Author.Age", ComparisonOperator.GreaterThan)]
    public int? MinimumAuthorAge { get; set; }

    [ComparativeWhereClause("PublishedYear", ComparisonOperator.GreaterThanOrEqual)]
    public int? FromYear { get; set; }

    [ComparativeWhereClause("PublishedYear", ComparisonOperator.LessThanOrEqual)]
    public int? ToYear { get; set; }

    [ComparativeWhereClause("Price", ComparisonOperator.LessThan)]
    public decimal? MaxPrice { get; set; }
}
```

### TextualWhereClause Attribute

Provides string-specific filtering methods.

#### Syntax
```csharp
[TextualWhereClause(StringMethod.Contains)]
[TextualWhereClause("PropertyName", StringMethod.StartsWith)]
[TextualWhereClause(StringMethod.EndsWith, LogicalOperator.OR)]
```

#### String Methods
- `Contains`: Property contains the specified text
- `NotContains`: Property does not contain the specified text
- `StartsWith`: Property starts with the specified text
- `NotStartsWith`: Property does not start with the specified text
- `EndsWith`: Property ends with the specified text
- `NotEndsWith`: Property does not end with the specified text

#### Examples
```csharp
public class TextualSearchRequest : IWhereClause
{
    [TextualWhereClause(StringMethod.Contains)]
    public string NameContains { get; set; }

    [TextualWhereClause("Description", StringMethod.NotContains)]
    public string ExcludeDescription { get; set; }

    [TextualWhereClause("Author.Name", StringMethod.StartsWith)]
    public string AuthorNamePrefix { get; set; }

    [TextualWhereClause("Title", StringMethod.EndsWith)]
    public string TitleSuffix { get; set; }
}
```

### WhereClauseClass Attribute

Applied to classes to configure class-level behavior (advanced usage).

## Usage Examples

### Basic Entity Filtering

```csharp
// Entity model
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Category Category { get; set; }
}

// Request class
public class ProductSearchRequest : IWhereClause
{
    [WhereClause]
    public int? Id { get; set; }

    [WhereClause]
    public string Name { get; set; }

    [ComparativeWhereClause(ComparisonOperator.LessThanOrEqual)]
    public decimal? MaxPrice { get; set; }

    [WhereClause("Category.Name")]
    public string CategoryName { get; set; }
}

// Usage in controller
[HttpPost("search")]
public IActionResult SearchProducts(ProductSearchRequest request)
{
    var products = _context.Products
        .Include(p => p.Category)
        .Where(request)
        .ToList();
    
    return Ok(products);
}
```

### Complex Nested Filtering

```csharp
// Entity models
public class Library
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Book> Books { get; set; }
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Author Author { get; set; }
    public List<Review> Reviews { get; set; }
}

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Country Country { get; set; }
}

// Advanced search request
public class LibrarySearchRequest : IWhereClause
{
    [WhereClause]
    public string Name { get; set; }

    [TextualWhereClause("Books.Title", StringMethod.Contains)]
    public string BookTitleContains { get; set; }

    [WhereClause("Books.Author.Name")]
    public string AuthorName { get; set; }

    [WhereClause("Books.Author.Country.Name")]
    public string AuthorCountry { get; set; }

    [ComparativeWhereClause("Books.Reviews.Count", ComparisonOperator.GreaterThan)]
    public int? MinimumReviews { get; set; }
}
```

### Combining Multiple Filter Types

```csharp
public class AdvancedBookSearchRequest : IWhereClause
{
    // Basic equality
    [WhereClause]
    public int? Id { get; set; }

    // String matching
    [TextualWhereClause(StringMethod.Contains)]
    public string Title { get; set; }

    // Comparative filtering
    [ComparativeWhereClause("PublishedYear", ComparisonOperator.GreaterThanOrEqual)]
    public int? FromYear { get; set; }

    [ComparativeWhereClause("PublishedYear", ComparisonOperator.LessThanOrEqual)]
    public int? ToYear { get; set; }

    // Range filtering
    [ComparativeWhereClause("Price", ComparisonOperator.GreaterThanOrEqual)]
    public decimal? MinPrice { get; set; }

    [ComparativeWhereClause("Price", ComparisonOperator.LessThanOrEqual)]
    public decimal? MaxPrice { get; set; }

    // Navigation properties
    [WhereClause("Author.Name")]
    public string AuthorName { get; set; }

    [TextualWhereClause("Author.Country.Name", StringMethod.StartsWith)]
    public string AuthorCountryPrefix { get; set; }

    // Logical operators
    [WhereClause("Category.Name", LogicalOperator.OR)]
    public string Category1 { get; set; }

    [WhereClause("Category.Name", LogicalOperator.OR)]
    public string Category2 { get; set; }
}
```

### Pagination with Filtering

```csharp
public class PagedBookSearchRequest : IWhereClause
{
    // Filter properties
    [TextualWhereClause(StringMethod.Contains)]
    public string Title { get; set; }

    [WhereClause("Author.Name")]
    public string AuthorName { get; set; }

    // Pagination properties (not filtered)
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// Usage with pagination
[HttpPost("search")]
public IActionResult SearchBooks(PagedBookSearchRequest request)
{
    var query = _context.Books
        .Include(b => b.Author)
        .Where(request); // Apply SmartWhere filters

    var totalCount = query.Count();
    
    var books = query
        .Skip((request.Page - 1) * request.PageSize)
        .Take(request.PageSize)
        .ToList();

    return Ok(new
    {
        Data = books,
        TotalCount = totalCount,
        Page = request.Page,
        PageSize = request.PageSize
    });
}
```

## Advanced Features

### Custom Property Mapping

When your request property names don't match entity property names:

```csharp
public class CustomMappingRequest : IWhereClause
{
    [WhereClause(PropertyName = "Id")]
    public int? ProductId { get; set; }

    [WhereClause(PropertyName = "Name")]
    public string ProductName { get; set; }

    [WhereClause("Category.Description")]
    public string CategoryDescription { get; set; }
}
```

### Logical Operators

Combine filters with AND/OR logic:

```csharp
public class LogicalOperatorRequest : IWhereClause
{
    // These will be combined with AND (default)
    [WhereClause]
    public string Name { get; set; }

    [WhereClause]
    public bool? IsActive { get; set; }

    // These will be combined with OR
    [WhereClause("Category.Name", LogicalOperator.OR)]
    public string Category1 { get; set; }

    [WhereClause("Category.Name", LogicalOperator.OR)]
    public string Category2 { get; set; }

    [WhereClause("Category.Name", LogicalOperator.OR)]
    public string Category3 { get; set; }
}
```

### Date Range Filtering

```csharp
public class DateRangeRequest : IWhereClause
{
    [ComparativeWhereClause("CreatedDate", ComparisonOperator.GreaterThanOrEqual)]
    public DateTime? StartDate { get; set; }

    [ComparativeWhereClause("CreatedDate", ComparisonOperator.LessThanOrEqual)]
    public DateTime? EndDate { get; set; }

    [ComparativeWhereClause("UpdatedDate", ComparisonOperator.GreaterThan)]
    public DateTime? UpdatedAfter { get; set; }
}
```

### Collection Property Filtering

```csharp
public class CollectionFilterRequest : IWhereClause
{
    // Filter by collection count
    [ComparativeWhereClause("Books.Count", ComparisonOperator.GreaterThan)]
    public int? MinimumBookCount { get; set; }

    // Filter by collection property
    [WhereClause("Books.Any(x => x.IsPublished)")]
    public bool? HasPublishedBooks { get; set; }
}
```

## API Reference

### Extension Methods

#### Where&lt;T&gt;(this IQueryable&lt;T&gt; source, IWhereClause whereClause)

Applies filtering to an `IQueryable<T>` based on the provided `IWhereClause` implementation.

**Parameters:**
- `source`: The `IQueryable<T>` to filter
- `whereClause`: The filter criteria implementing `IWhereClause`

**Returns:** Filtered `IQueryable<T>`

**Example:**
```csharp
var filteredQuery = _context.Products.Where(searchRequest);
```

### Enums

#### ComparisonOperator

Defines comparison operations for numeric and date filtering:

```csharp
public enum ComparisonOperator
{
    Equal,
    NotEqual,
    GreaterThan,
    NotGreaterThan,
    GreaterThanOrEqual,
    NotGreaterThanOrEqual,
    LessThan,
    NotLessThan,
    LessThanOrEqual,
    NotLessThanOrEqual
}
```

#### StringMethod

Defines string matching methods:

```csharp
public enum StringMethod
{
    Contains,
    NotContains,
    StartsWith,
    NotStartsWith,
    EndsWith,
    NotEndsWith
}
```

#### LogicalOperator

Defines logical operators for combining filters:

```csharp
public enum LogicalOperator
{
    AND,
    OR
}
```

## Best Practices

### 1. Nullable Properties

Always use nullable types for filter properties to distinguish between "no filter" and "filter by default value":

```csharp
// Good
[WhereClause]
public int? Id { get; set; }

[WhereClause]
public bool? IsActive { get; set; }

// Avoid
[WhereClause]
public int Id { get; set; } // Will always filter, even when 0
```

### 2. Meaningful Property Names

Use descriptive property names that clearly indicate their purpose:

```csharp
// Good
public class ProductSearchRequest : IWhereClause
{
    [ComparativeWhereClause(ComparisonOperator.GreaterThanOrEqual)]
    public decimal? MinPrice { get; set; }

    [ComparativeWhereClause(ComparisonOperator.LessThanOrEqual)]
    public decimal? MaxPrice { get; set; }
}

// Less clear
public class ProductSearchRequest : IWhereClause
{
    [ComparativeWhereClause(ComparisonOperator.GreaterThanOrEqual)]
    public decimal? Price1 { get; set; }

    [ComparativeWhereClause(ComparisonOperator.LessThanOrEqual)]
    public decimal? Price2 { get; set; }
}
```

### 3. Include Necessary Navigation Properties

Ensure your Entity Framework query includes the navigation properties you're filtering on:

```csharp
// Correct - includes Author for filtering
var books = _context.Books
    .Include(b => b.Author)
    .Where(request)
    .ToList();

// Incorrect - will cause runtime error if filtering by Author.Name
var books = _context.Books
    .Where(request) // Error if request filters by Author.Name
    .ToList();
```

### 4. Performance Considerations

- Use indexes on frequently filtered properties
- Be mindful of complex navigation property filters
- Consider pagination for large result sets
- Use projections when you don't need all properties

```csharp
// Good for performance
var bookTitles = _context.Books
    .Where(request)
    .Select(b => new { b.Id, b.Title })
    .ToList();
```

### 5. Validation

Add validation to your request classes:

```csharp
public class ProductSearchRequest : IWhereClause
{
    [WhereClause]
    [Range(1, int.MaxValue)]
    public int? Id { get; set; }

    [TextualWhereClause(StringMethod.Contains)]
    [StringLength(100)]
    public string Name { get; set; }

    [ComparativeWhereClause(ComparisonOperator.GreaterThanOrEqual)]
    [Range(0, double.MaxValue)]
    public decimal? MinPrice { get; set; }
}
```

## Troubleshooting

### Common Issues

#### 1. Property Not Found Exception

**Error:** Property 'PropertyName' not found on type 'EntityType'

**Solution:** Ensure the property name in the attribute matches the entity property name exactly:

```csharp
// Wrong
[WhereClause(PropertyName = "book_name")] // Entity has BookName
public string BookName { get; set; }

// Correct
[WhereClause(PropertyName = "BookName")]
public string BookName { get; set; }
```

#### 2. Navigation Property Errors

**Error:** Unable to translate the given 'Where' expression

**Solution:** Include necessary navigation properties in your query:

```csharp
// Add Include for navigation properties
var result = _context.Publishers
    .Include(p => p.Books)
    .ThenInclude(b => b.Author)
    .Where(request)
    .ToList();
```

#### 3. Type Mismatch

**Error:** Cannot convert type 'string' to 'int'

**Solution:** Ensure request property types match entity property types:

```csharp
// Entity property: public int Year { get; set; }
// Request property should be:
[WhereClause]
public int? Year { get; set; } // Not string
```

#### 4. No Results Returned

**Issue:** Query returns no results when it should

**Solution:** 
- Check that request properties have values
- Verify property names match exactly
- Ensure navigation properties are included
- Check for case sensitivity issues

### Debugging Tips

1. **Log Generated SQL:** Enable Entity Framework logging to see generated SQL queries

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.LogTo(Console.WriteLine);
}
```

2. **Test with Simple Filters:** Start with basic equality filters before using complex ones

3. **Verify Entity Relationships:** Ensure your entity relationships are properly configured

## Contributing

We welcome contributions to SmartWhere! Here's how you can help:

### Getting Started

1. Fork the repository
2. Clone your fork locally
3. Create a feature branch
4. Make your changes
5. Add tests for new functionality
6. Ensure all tests pass
7. Submit a pull request

### Development Setup

```bash
# Clone the repository
git clone https://github.com/byerlikaya/SmartWhere.git

# Navigate to the project directory
cd SmartWhere

# Restore dependencies
dotnet restore

# Run tests
dotnet test

# Build the project
dotnet build
```

### Contribution Guidelines

- Follow existing code style and conventions
- Add unit tests for new features
- Update documentation for new functionality
- Ensure backward compatibility when possible
- Write clear commit messages

### Reporting Issues

When reporting issues, please include:

- SmartWhere version
- .NET version
- Entity Framework version (if applicable)
- Minimal reproduction code
- Expected vs actual behavior

## License

SmartWhere is released under the MIT License. See the [LICENSE](LICENSE) file for details.

### MIT License Summary

- ✅ Commercial use
- ✅ Modification
- ✅ Distribution
- ✅ Private use
- ❌ Liability
- ❌ Warranty

---

## Support

- **GitHub Issues:** [Report bugs or request features](https://github.com/byerlikaya/SmartWhere/issues)
- **GitHub Wiki:** [Additional documentation and examples](https://github.com/byerlikaya/SmartWhere/wiki)
- **NuGet Package:** [Download and install](https://www.nuget.org/packages/SmartWhere)

---

**SmartWhere** - Making LINQ queries smarter, one filter at a time! ⭐

If you find this library helpful, please consider giving it a star on GitHub to help others discover it.