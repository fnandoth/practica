using OrderServices.Domain.Entities;
using OrderServices.Aplication.DTOs;

namespace OrderServices.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDTOs>> GetOrdersAsync();
        Task<OrderResponseDTO> GetOrderByIdAsync(string id);
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserNameAsync(string username);
        Task<OrderDTOs> CreateOrderAsync(OrderDTOs order);
        Task<List<OrderProduct>> AddProductAsync(List<OrderProductDTO> products, Guid id);
        Task<bool> UpdateOrderAsync(OrderDTOs order, string id);
        Task<bool> DeleteOrderAsync(string id);

        Task<int> SaveChangesAsync();
    }
}