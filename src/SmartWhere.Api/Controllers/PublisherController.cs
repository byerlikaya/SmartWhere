using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWhere.Api.Entities;
using SmartWhere.Api.Requests;

namespace SmartWhere.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public PublisherController(ProjectDbContext context)
        {
            PublisherData.FillDummyData();
            _context = context;
        }

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
    }
}