using Microsoft.AspNetCore.Mvc;
using SmartWhere.Sample.Api.ApplicationSpecific;
using SmartWhere.Sample.Api.Requests;

namespace SmartWhere.Sample.Api.Controllers
{

    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public ProductController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpPost("/products")]
        public IActionResult GetPublishers(ProductSearchRequest request)
        {
            var result = _context.Products
                .Where(request)
                .ToList();

            return Ok(result);
        }
    }
}