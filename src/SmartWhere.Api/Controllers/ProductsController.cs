using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartWhere.Sample.Api.ApplicationSpecific.Contexts;
using SmartWhere.Sample.Api.Requests;

namespace SmartWhere.Sample.Api.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MSSqlContext _context;

        public ProductsController(MSSqlContext context)
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