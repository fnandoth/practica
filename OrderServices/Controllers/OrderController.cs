using Microsoft.AspNetCore.Mvc;
using OrderServices.Domain.Interfaces;
using OrderServices.Domain.Entities;
using OrderServices.Aplication.DTOs;

namespace OrderServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTOs>>> GetOrders()
        {
            var orders = await _orderRepository.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("id/{id:guid}")]
        public async Task<ActionResult<OrderDTOs>> GetOrder(Guid id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<OrderDTOs>> GetOrderByUserName(string username)
        {
            var order = await _orderRepository.GetOrdersByUserNameAsync(username);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }


        [HttpPost]
        public async Task<ActionResult<OrderDTOs>> CreateOrder(OrderDTOs order)
        {
            await _orderRepository.CreateOrderAsync(order);
            await _orderRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrders), order);
        }
    }
}