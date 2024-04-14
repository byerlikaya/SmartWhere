using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sample.Api.ApplicationSpecific.Contexts;
using Sample.Common.Dto;
using Sample.Common.Entity;
using SmartWhere;

namespace Sample.Api.Controllers;

[ApiController]
public class PublisherController(MemoryDbContext context) : ControllerBase
{
    [HttpPost("/publishers")]
    public IActionResult GetPublishers(PublisherSearchRequest request)
    {
        var result = context.Set<Publisher>()
            .Include(x => x.Books)
            .ThenInclude(x => x.Author)
            .Where(request)
            .ToList();

        return Ok(result);
    }

    [HttpPost("/books")]
    public IActionResult GetBooks(BookSearchRequest request)
    {
        var result = context.Books
            .Include(x => x.Author)
            .Where(request)
            .Skip(request.Start)
            .Take(request.Max)
            .ToList();

        return Ok(result);
    }
}