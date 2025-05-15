namespace OrderServices.Aplication.DTOs
{
    public class OrderDTOs
    {
        public string UserName { get; set; } = null!;
        public List<OrderProductDTO> Products { get; set; } = null!;
    }

    public class OrderResponseDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public List<ProductResponseDTO> Products { get; set; } = null!;
        public decimal TotalPrice { get; set; }
    }

    
}