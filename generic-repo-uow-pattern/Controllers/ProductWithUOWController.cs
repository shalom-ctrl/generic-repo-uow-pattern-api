using generic_repo_uow_pattern.Dto;
using generic_repo_uow_pattern.Interface;
using generic_repo_uow_pattern.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace generic_repo_uow_pattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithUOWController : ControllerBase
    {
        private readonly IUnitOfWork _work;

        public ProductWithUOWController(IUnitOfWork work)
        {
            _work = work;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _work.GetRepository<Product>().GetAllAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductRequest product)
        {
            try
            {
                using var transaction = _work.BeginTransactionAsync();

                var newProduct = new Product
                {
                    ProductName = product.ProductName,
                    Price = product.Price
                };

                var productresult = await _work.GetRepository<Product>().AddAsync(newProduct);
                await _work.SaveChangesAsync();

                var order = new Order
                {
                    ProductId = productresult.ProductId,
                    OrderDate = DateTime.Now
                };

                await _work.GetRepository<Order>().AddAsync(order);
                await _work.SaveChangesAsync();

                await _work.CommitTransactionAsync();

                var responseDto = new ProductResponse
                {
                    ProductId = productresult.ProductId,
                    ProductName = productresult.ProductName,
                    Price = productresult.Price
                };

                return StatusCode(StatusCodes.Status201Created, responseDto);
            }
            catch (Exception ex)
            {
                await _work.RollBackTransactionAsync();
                throw;
            }
        }
    }
}
