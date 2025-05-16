using Microsoft.EntityFrameworkCore;
using ProductServices.Domain.Interfaces;
using ProductServices.Domain.Entities;
using System.Diagnostics;
using ProductServices.Aplication.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ProductServices.Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(ProductContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductResponseDTO> GetProductByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                throw new ArgumentException($"El ID {id} no es un GUID válido");
            }
            var product = await _context.Products.FindAsync(guidId)
                ?? throw new KeyNotFoundException($"Producto con ID {guidId} no encontrado");

            return _mapper.Map<ProductResponseDTO>(product);
        }
        public async Task<ProductResponseDTO> GetProductByNameAsync(string name)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
            if (product == null)
                return null!;
            return _mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<ProductResponseDTO> AddProductAsync(ProductDTO product)
        {
            var existingProduct = await GetProductByNameAsync(product.Name);
            if (existingProduct != null)
            {
                throw new InvalidOperationException($"Ya existe un producto con el nombre {product.Name}");
            }
            var productEntity = _mapper.Map<Product>(product);
            productEntity.Id = Guid.NewGuid();
            await _context.Products.AddAsync(productEntity);
            await SaveChangesAsync();
            return _mapper.Map<ProductResponseDTO>(product);
        }
        public async Task<bool> UpdateProductAsync(string name, ProductDTO product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Name == name)
                ?? throw new KeyNotFoundException($"Producto con nombre {name} no encontrado");
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            _context.Products.Update(existingProduct);
            await SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteProductAsync(string name)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == name)
                ?? throw new KeyNotFoundException($"Producto con nombre {name} no encontrado");
            _context.Products.Remove(product);
            await SaveChangesAsync();
            return true;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}