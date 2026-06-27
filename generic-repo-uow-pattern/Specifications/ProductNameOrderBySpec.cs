using generic_repo_uow_pattern.Models;

namespace generic_repo_uow_pattern.Specifications
{
    public class ProductNameOrderBySpec : Specification<Product>
    {
        public ProductNameOrderBySpec(string name) : base(x => x.ProductName.Contains(name))
        {
            ApplyOrderBy(x => x.ProductName);
            //AddInclude(x => x.Orders);
        }
    }
}
