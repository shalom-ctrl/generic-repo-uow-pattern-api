namespace generic_repo_uow_pattern.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
