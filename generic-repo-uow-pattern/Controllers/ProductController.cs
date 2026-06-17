using generic_repo_uow_pattern.Dto;
using generic_repo_uow_pattern.Interface;
using generic_repo_uow_pattern.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace generic_repo_pattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly generic_repo_uow_pattern.Interface.IRepository<Product> _productRepository;

        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest product)
        {
            var productEntity = new Product ()
            {
                ProductName = product.ProductName,
                Price = product.Price
            };
            var createdProduct = await _productRepository.AddAsync(productEntity);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductRequest product)
        {
            var productEntity = await _productRepository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            productEntity.ProductName = product.ProductName;
            productEntity.Price = product.Price;

            await _productRepository.UpdateAsync(productEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productEntity = await _productRepository.GetByIdAsync(id);
            if (productEntity == null)
            {
                return NotFound();
            }
            await _productRepository.DeleteAsync(productEntity);
            return NoContent();
        }
    }
}
