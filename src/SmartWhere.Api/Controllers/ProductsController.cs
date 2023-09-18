using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWhere.Sample.Api.ApplicationSpecific;
using SmartWhere.Sample.Api.Requests;

namespace SmartWhere.Sample.Api.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProjectDbContext _context;

        public ProductsController(ProjectDbContext context)
        {
            _context = context;
        }

        [HttpPost("/products")]
        public IActionResult Products(ProductSearchRequest request)
        {
            var result = _context.Products
                .Include(x => x.Category)
                .Where(request)
                .ToList();

            return Ok(result);
        }
    }
}