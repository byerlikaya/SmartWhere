using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWhere.Sample.Api.Entities;
using SmartWhere.Sample.Api.Requests;

namespace SmartWhere.Sample.Api.Controllers
{

    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public SampleController(ProjectDbContext context)
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