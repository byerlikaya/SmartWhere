using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWhere.Sample.Api.ApplicationSpecific.Contexts;
using SmartWhere.Sample.Common.Dto;
using SmartWhere.Sample.Common.Entity;

namespace SmartWhere.Sample.Api.Controllers
{
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly MemoryContext _context;

        public PublisherController(MemoryContext context)
        {
            _context = context;
        }

        [HttpPost("/publishers")]
        public IActionResult GetPublishers(PublisherSearchRequest request)
        {
            var result = _context.Set<Publisher>()
                .Include(x => x.Books)
                .ThenInclude(x => x.Author)
                .Where(request)
                .ToList();

            return Ok(result);
        }

        [HttpPost("/books")]
        public IActionResult GetBooks(BookSearchRequest request)
        {
            var result = _context.Set<Book>()
                .Include(x => x.Author)
                .Where(request)
                .Skip(request.Start)
                .Take(request.Max)
                .ToList();

            return Ok(result);
        }
    }
}
