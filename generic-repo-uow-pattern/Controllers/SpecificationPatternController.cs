using generic_repo_uow_pattern.Interface;
using generic_repo_uow_pattern.Models;
using generic_repo_uow_pattern.Repository;
using generic_repo_uow_pattern.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace generic_repo_uow_pattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationPatternController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecificationPatternController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetProductsByNameSpecification")]
        public async Task<IActionResult> GetProductsByNameSpec(string name)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new ProductNameSpec(name);
            var results = await productRepository.FindAsync(spec);
            return Ok(results);
        }

        [HttpGet("GetProductsByIdentification")]
        public async Task<IActionResult> GetProductsByIdentification(int id)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new ProductIdSpec(id);
            var results = await productRepository.FindAsync(spec);
            return Ok(results);
        }

        [HttpGet("GetProductsByNameOrderBySpec")]
        public async Task<IActionResult> GetProductsByNameOrderBySpec(string search)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new ProductNameOrderBySpec(search);
            var results = await productRepository.FindAsync(spec);
            return Ok(results);
        }

        [HttpGet("GetProductsByNameOrderByDescSpec")]
        public async Task<IActionResult> GetProductsByNameOrderByDescSpec(string search)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new ProductNameOrderByDescSpec(search);
            var results = await productRepository.FindAsync(spec);
            return Ok(results);
        }

        [HttpGet("GetProductsByNameOrderByPagingspec")]
        public async Task<IActionResult> GetProductsByNameOrderByPagingspec(string search, int pageNumber, int pageSize)
        {
            var productRepository = _unitOfWork.GetRepository<IProductRepository, Product>();
            var spec = new ProductNameOrderByPagingspec(search, pageNumber, pageSize);
            var results = await productRepository.FindAsync(spec);
            return Ok(results);
        }
    }
}
