using Microsoft.AspNetCore.Mvc;
using WebApi.ReflectionDemo.Models;

namespace WebApi.ReflectionDemo.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private static readonly List<ProductDto> _products = new();

        // GET: api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_products);
        }

        // GET: api/products/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public IActionResult Add(ProductDto product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _products.Add(product);
            return Ok(product);
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            _products.Remove(product);
            return NoContent();
        }
    }
}