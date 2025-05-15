using OrderServices.Domain.Entities;
using OrderServices.Aplication.DTOs;

namespace OrderServices.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDTOs>> GetOrdersAsync();
        Task<OrderDTOs> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserNameAsync(string username);
        Task<OrderDTOs> CreateOrderAsync(OrderDTOs order);
        Task<List<OrderProduct>> AddProductAsync(List<OrderProductDTO> products, Guid id);
        Task<bool> UpdateOrderAsync(OrderDTOs order, Guid id);
        Task<bool> DeleteOrderAsync(Guid id);

        Task<int> SaveChangesAsync();
    }
}