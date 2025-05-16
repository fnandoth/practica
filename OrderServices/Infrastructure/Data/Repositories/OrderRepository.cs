using Microsoft.EntityFrameworkCore;
using OrderServices.Domain.Interfaces;
using OrderServices.Domain.Entities;
using OrderServices.Aplication.DTOs;
using AutoMapper;
using System.Text.Json;
namespace OrderServices.Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        public OrderRepository(OrderContext context, IMapper mapper, HttpClient httpClient)
        {
            _context = context;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<OrderDTOs>> GetOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return _mapper.Map<IEnumerable<OrderDTOs>>(orders);
        }


        public async Task<OrderResponseDTO> GetOrderByIdAsync(string id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id.ToString() == id)
                ?? throw new KeyNotFoundException($"Orden {id} no encontrada");
            var products = await _context.OrderProducts.Where(p => p.OrderId == order.Id).ToListAsync();
            order.Products = products;
            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserNameAsync(string username)
        {
            var orders = await _context.Orders.Where(o=> o.UserName == username).ToListAsync()
                ?? throw new KeyNotFoundException($"No se encontraron ordenes para el usuario {username}");
            foreach (var order in orders)
            {
                var products = await _context.OrderProducts.Where(p => p.OrderId == order.Id).ToListAsync();
                order.Products = products;
            }
            return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }
 
        public async Task<OrderDTOs> CreateOrderAsync(OrderDTOs order)
        {
            // llamar a la api de user y verificar que el usuario existe
            var userExists = await _httpClient.GetAsync($"http://gateway:8080/api/user/name/{order.UserName}");
            if (!userExists.IsSuccessStatusCode)
                throw new KeyNotFoundException($"El usuario {order.UserName} no existe");
            // se mapea la lista de productos para verificar que existen y se obtienen los ids y precios
            var OrderID = Guid.NewGuid();
            var ProductsList = await AddProductAsync(order.Products, OrderID);
            var orderEntity = new Order
            {
                Id = OrderID,
                UserName = order.UserName,
                Products = ProductsList,
                TotalPrice = ProductsList.Sum(p => p.Price * p.Quantity)
            };
            _context.Orders.Add(orderEntity);

            await SaveChangesAsync();
            return _mapper.Map<OrderDTOs>(orderEntity);
        }

        public async Task<List<OrderProduct>> AddProductAsync(List<OrderProductDTO> Products, Guid Orderid)
        {
            var productList = new List<OrderProduct>();
            foreach (var product in Products) // esto se puede simplificar usando el auto mapper
            {
                var productExists = await _httpClient.GetAsync($"http://gateway:8080/api/product/name/{product.ProductName}"); // verificar que el producto existe
                // si el producto no existe, lanzar una excepcion
                if (!productExists.IsSuccessStatusCode)
                    throw new KeyNotFoundException($"El producto {product.ProductName} no existe");
                // si el producto existe, obtener el id y el precio
                // deserializar el json
                var content = await productExists.Content.ReadAsStringAsync();
                var productData = JsonSerializer.Deserialize<JsonDocument>(content)
                    ?? throw new JsonException("Error al deserializar la respuesta del producto");
                var ProductData = new OrderProduct();
                try
                {
                    var id = productData.RootElement.GetProperty("id").GetString()
                        ?? throw new KeyNotFoundException($"El id del producto {product.ProductName} no existe");
                    var name = productData.RootElement.GetProperty("name").GetString()
                        ?? throw new KeyNotFoundException($"El nombre del producto {product.ProductName} no existe");
                    var price = productData.RootElement.GetProperty("price").GetDecimal();
                    var quantity = product.Quantity;
                    ProductData.OrderId = Orderid;
                    ProductData.ProductId = id;
                    ProductData.ProductName = name;
                    ProductData.Price = price;
                    ProductData.Quantity = quantity;
                }
                catch (KeyNotFoundException ex)
                {
                    throw new KeyNotFoundException($"Error al obtener el producto {product.ProductName}: {ex.Message}");
                }
                // si no hay excepciones, agregar el producto a la lista
                productList.Add(ProductData);
            }
            
            return productList;
        }

        public async Task<bool> UpdateOrderAsync(OrderDTOs order, string id)
        {
            var orderEntity = _context.Orders.FirstOrDefault(o => o.Id.ToString() == id)
                ?? throw new KeyNotFoundException($"Orden {id} no encontrada");
            // verificar que el usuario existe
            var userExists = await _httpClient.GetAsync($"http://gateway:8080/api/user/name/{order.UserName}");
            if (!userExists.IsSuccessStatusCode)
                throw new KeyNotFoundException($"El usuario {order.UserName} no existe");
            // se mapea la lista de productos para verificar que existen y se obtienen los ids y precios
            var ProductsList = await UpdateProductAsync(orderEntity.Products, order.Products, orderEntity.Id);
            orderEntity.UserName = order.UserName;
            orderEntity.Products = ProductsList;
            orderEntity.TotalPrice = ProductsList.Sum(p => p.Price * p.Quantity);
            _context.Orders.Update(orderEntity);
            await SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderProduct>> UpdateProductAsync(List<OrderProduct> OProducts, List<OrderProductDTO> LProducts, Guid id)
        {
            // verificar si hay cambios en la lista
            var productList = await AddProductAsync(LProducts, id);
            foreach (var product in productList)
            {
                var productExists = OProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (productExists != null)
                {
                    productExists.Quantity = product.Quantity;
                    productExists.Price = product.Price;
                }
                else
                {
                    OProducts.Add(product);
                }
            }
            // eliminar los productos que no estan en la lista de productos 
            var toRemove = OProducts
                .Where(op => !productList.Any(p => p.ProductId == op.ProductId))
                .ToList();
            // eliminar los productos que no estan en la lista de productos, si hubiera eliminado directamente desde el foreach obtendria un error de enumeracion
            foreach (var remove in toRemove) 
            {
                OProducts.Remove(remove);
            }
            return OProducts;
            
        }

        public async Task<bool> DeleteOrderAsync(string id)
        {
            var orderEntity = _context.Orders.FirstOrDefault(o => o.Id.ToString() == id)
                ?? throw new KeyNotFoundException($"Orden {id} no encontrada");
            _context.Orders.Remove(orderEntity);
            await SaveChangesAsync();
            return true;
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}