namespace OrderServices.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = null!;
        public List<OrderProduct> Products { get; set; } = [];
        public decimal TotalPrice { get; set; }
    }
}