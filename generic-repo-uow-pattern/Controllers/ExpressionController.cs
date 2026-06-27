using generic_repo_uow_pattern.Interface;
using generic_repo_uow_pattern.Models;
using generic_repo_uow_pattern.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace generic_repo_uow_pattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpressionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExpressionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("SearchIncludeOrderPagination")]
        public async Task<IActionResult> GetAllProductWithPagination(string? name = null, int? pageNumber = null, int? pageSize = null)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var results = await productRepository.SearchOrderAndPaginateAsync
                (!string.IsNullOrWhiteSpace(name) ? x => x.ProductName.Contains(name) : null, x => x.ProductId, false, pageNumber, pageSize);

            return Ok(new { results.Result, results.TotalNumber, results.TotalPages, results.IsNext, results.IsPrevious });
        }

        [HttpGet("GetProductbyNamePrice")]
        public async Task<IActionResult> GetProductByNamePrice(string name, decimal price)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();

            var results = await productRepository.FindAsync(x => x.ProductName == name && x.Price == price);
            return Ok(results);
        }

        [HttpGet("GetProductByName")]
        public async Task<IActionResult> GetProductByName (string name)
        {
            var product = _unitOfWork.GetRepository<IProductRepository, Product>();

            var results = await product.FindAllAsync(x => x.ProductName.Contains(name));
            return Ok(results);
        }

        [HttpGet("GetProductCountWithName")]
        public async Task<IActionResult> GetProductCountWithName(string name)
        {
            var product = _unitOfWork.GetRepository<IProductRepository, Product>();

            var results = await product.CountAsync(x => x.ProductName.Contains(name));
            return Ok(results);
        }

        [HttpGet("GetProductCount")]
        public async Task<IActionResult> GetProductCount()
        {
            var product = _unitOfWork.GetRepository<IProductRepository, Product>();

            var results = await product.CountAsync();
            return Ok(results);
        }
    }
}
