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

        public async Task<IEnumerable<Product>> GetProductsByName(string productName)
        {
            return await _dbSet.Where(p => p.ProductName.Contains(productName)).ToListAsync();
        }
    }
}
