using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApiExample
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductAdapter _adapter;

        public ProductController(ProductAdapter adapter)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _adapter.GetProduct(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}