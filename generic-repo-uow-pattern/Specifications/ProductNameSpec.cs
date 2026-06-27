using generic_repo_uow_pattern.Models;
using generic_repo_uow_pattern.Specifications;

namespace generic_repo_uow_pattern.Specifications
{
    public class ProductNameSpec : Specification<Product>
    {
        public ProductNameSpec(string productName) : base(x => x.ProductName.Contains(productName))
        {
            
        }
    }
}
