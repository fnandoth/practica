using System;

namespace OrderServices.Domain.Entities;

public class OrderProduct
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OrderId { get; set; }
    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public Order Order { get; set; } = null!;
}