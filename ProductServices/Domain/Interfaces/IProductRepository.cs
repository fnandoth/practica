using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductServices.Aplication.DTOs;
using ProductServices.Domain.Entities;

namespace ProductServices.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductResponseDTO> GetProductByIdAsync(string id);
        Task<ProductResponseDTO> GetProductByNameAsync(string name);
        Task<ProductResponseDTO> AddProductAsync(ProductDTO product);
        Task<bool> UpdateProductAsync(string name, ProductDTO product);
        Task<bool> DeleteProductAsync(string id);
        Task<int> SaveChangesAsync();
    }
}