using Microsoft.AspNetCore.Mvc;
using ProductServices.Domain.Interfaces;
using ProductServices.Domain.Entities;
using ProductServices.Aplication.DTOs;

namespace ProductServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductByName(string name)
        {
            var product = await _productRepository.GetProductByNameAsync(name);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> CreateProduct(ProductDTO product)
        {
            await _productRepository.AddProductAsync(product);
            await _productRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), product);
            
        }
        [HttpDelete("name/{name}")]
        public async Task<IActionResult> DeleteProduct(string name)
        {
            await _productRepository.DeleteProductAsync(name);
            await _productRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}