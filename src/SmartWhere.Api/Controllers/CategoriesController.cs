using Microsoft.AspNetCore.Mvc;
using SmartWhere.Sample.Api.ApplicationSpecific;
using SmartWhere.Sample.Api.Requests;

namespace SmartWhere.Sample.Api.Controllers
{
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MSSqlContext _context;

        public CategoriesController(MSSqlContext context)
        {
            _context = context;
        }

        [HttpPost("/categories")]
        public IActionResult Categories(CategorySearchRequest request)
        {
            var result = _context.Categories
                .Where(request)
                .ToList();

            return Ok(result);
        }
    }
}
