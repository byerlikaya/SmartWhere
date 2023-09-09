# SmartWhere
#### It intelligently filters a sequence of items in a simple and practical way.

![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/byerlikaya/SmartWhere/dotnet.yml)
[![SmartWhere Nuget](https://img.shields.io/nuget/v/SmartWhere)](https://www.nuget.org/packages/SmartWhere)
[![SmartWhere Nuget](https://img.shields.io/nuget/dt/SmartWhere)](https://www.nuget.org/packages/SmartWhere)

**SmartWhere** is a method that aims to make the `Enumerable.Where` method smarter and is based on the foundations of .NET Core.

#### Quick Start
The usage of **SmartWhere** is quite simple.

1. Install `SmartWhere` NuGet package from [here](https://www.nuget.org/packages/SmartWhere/).

````
PM> Install-Package SmartWhere
````

2. You need to define our Request object for **SmartWhere** and sign it with the `IWhereClause` interface.
   
```csharp
 public class PublisherSearchRequest : IWhereClause
```

3. We mark the properties to be included in the filter with the `WhereClause` Attribute.

```csharp
public class PublisherSearchRequest : IWhereClause
{
    [WhereClause]
    public int Id { get; set; }

    [WhereClause(PropertyName = "Name"]
    public string PublisherName { get; set; }

    [WhereClause("Book.Name")]
    public string BookName { get; set; }

    [WhereClause("Books.Author.Name")]
    public string AuthorName { get; set; }
}
```
4. And we filter smartly. That's it.

```csharp
 [HttpPost]
 public IActionResult GetPublisher(PublisherSearchRequest request)
 {
     var result = _context.Set<Publisher>()
         .Include(x => x.Books)
         .ThenInclude(x => x.Author)
         .Where(request)
         .ToList();

     return Ok(result);
 }
```

Be sure to check out the [Wiki page](https://github.com/byerlikaya/SmartWhere/wiki) for more details
