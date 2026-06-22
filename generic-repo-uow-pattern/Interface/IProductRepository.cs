using generic_repo_uow_pattern.Common;
using generic_repo_uow_pattern.Models;

namespace generic_repo_uow_pattern.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByName(string productName);
        Task<PaginatedList<Product>> GetAllProductsWithPaging(int page, int pageSize, string SearchTerm);
    }
}
