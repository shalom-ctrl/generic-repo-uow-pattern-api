using generic_repo_uow_pattern.Dto;
using generic_repo_uow_pattern.Interface;
using generic_repo_uow_pattern.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace generic_repo_uow_pattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWithUOWController : ControllerBase
    {
        private readonly IUnitOfWork _unitofwork;

        public ProductWithUOWController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productRepository = _unitofwork.GetRepository<IProductRepository, Product>();
            var products = await productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("productbyName")]
        public async Task<IActionResult> GetByName(string productName)
        {
            var productRepository = _unitofwork.GetRepository<IProductRepository, Product>();
            var result = await productRepository.GetProductsByName(productName);
           // var product = await _unitofwork.ProductRepository.GetProductsByName(productName);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductRequest product)
        {
            try
            {
                using var transaction = _unitofwork.BeginTransactionAsync();

                var newProduct = new Product
                {
                    ProductName = product.ProductName,
                    Price = product.Price
                };

                var productRepository = _unitofwork.GetRepository<IProductRepository, Product>();
                var productresult = await productRepository.AddAsync(newProduct);
                await _unitofwork.SaveChangesAsync();

                var order = new Order
                {
                    ProductId = productresult.ProductId,
                    OrderDate = DateTime.Now
                };

                await _unitofwork.GetRepository<Order>().AddAsync(order);
                await _unitofwork.SaveChangesAsync();

                await _unitofwork.CommitTransactionAsync();

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
                await _unitofwork.RollBackTransactionAsync();
                throw;
            }
        }
    }
}
