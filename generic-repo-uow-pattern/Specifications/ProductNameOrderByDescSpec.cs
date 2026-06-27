using generic_repo_uow_pattern.Models;

namespace generic_repo_uow_pattern.Specifications
{
    public class ProductNameOrderByDescSpec : Specification<Product>
    {
        public ProductNameOrderByDescSpec(string name) : base(x => x.ProductName.Contains(name))
        {
            ApplyOrderByDescending(x => x.ProductName);
        }
    }
}
