using generic_repo_uow_pattern.Models;

namespace generic_repo_uow_pattern.Specifications
{
    public class ProductNameOrderByPagingspec : Specification<Product>
    {
        public ProductNameOrderByPagingspec(string name, int pageNumber, int pageSize) : base(x => x.ProductName.Contains(name))
        {
            ApplyOrderBy(x => x.ProductName);
            ApplyPaging(pageNumber, pageSize);
        }
    }
}
