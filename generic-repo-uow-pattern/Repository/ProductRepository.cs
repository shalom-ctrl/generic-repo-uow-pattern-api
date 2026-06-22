using generic_repo_uow_pattern.Common;
using generic_repo_uow_pattern.Data;
using generic_repo_uow_pattern.Interface;
using generic_repo_uow_pattern.Models;
using Microsoft.EntityFrameworkCore;

namespace generic_repo_uow_pattern.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PaginatedList<Product>> GetAllProductsWithPaging(int page, int pageSize, string SearchTerm)
        {
            IQueryable<Product> query = _dbSet
                .Include(p => p.Orders);

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(p => EF.Functions.Like(p.ProductName, $"%{SearchTerm}%"));
            }

            var Result = await PaginatedList<Product>.ToPagedList(query.OrderBy(x => x.ProductId), page, pageSize);
            return Result;
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string productName)
        {
            return await _dbSet.Where(p => p.ProductName.Contains(productName)).ToListAsync();
        }
    }
}
