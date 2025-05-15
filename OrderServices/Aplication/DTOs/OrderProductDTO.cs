using System;

namespace OrderServices.Aplication.DTOs{
    public class OrderProductDTO
    {
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
    }

    public class ProductResponseDTO
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}