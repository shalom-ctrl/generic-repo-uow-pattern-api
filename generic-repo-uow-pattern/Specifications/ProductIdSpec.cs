using generic_repo_uow_pattern.Models;
using generic_repo_uow_pattern.Specifications;

namespace generic_repo_uow_pattern.Specifications
{
    public class ProductIdSpec : Specification<Product>
    {
        public ProductIdSpec(int id) : base(x => x.ProductId == id )
        {

        }
    }
}
