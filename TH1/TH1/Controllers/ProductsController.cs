using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TH1.DTOs;
using TH1.Services;
using TH1.Patterns.Singleton;

namespace TH1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
            LoggerService.Instance.Log("ProductsController initialized.");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
            {
                LoggerService.Instance.Log($"Product with id {id} not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            var newProduct = await _productService.CreateProduct(productDto);
            LoggerService.Instance.Log($"Product {newProduct.Name} created.");
            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductId }, newProduct);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(int id, ProductDto productDto)
        {
            var updatedProduct = await _productService.UpdateProduct(id, productDto);
            if (updatedProduct == null)
            {
                 LoggerService.Instance.Log($"Product with id {id} not found for update.");
                return NotFound();
            }
            LoggerService.Instance.Log($"Product {updatedProduct.Name} updated.");
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);
            LoggerService.Instance.Log($"Product with id {id} deleted.");
            return NoContent();
        }
    }
}
