using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.DTOs.Responses;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ProductResponse> GetMainGameProductAsync()
        {
            var product = await _productRepo.GetMainGameProductAsync();
            return new ProductResponse { Id = product.Id, Name = product.Name, Price = product.Price };
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepo.GetAllAsync();
            return products.Select(p => new ProductResponse { Id = p.Id, Name = p.Name, Price = p.Price });
        }

        public async Task<ProductResponse?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return null;
            return new ProductResponse { Id = product.Id, Name = product.Name, Price = product.Price };
        }

        public async Task<ProductResponse> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = createProductDto.Name,
                Price = createProductDto.Price
            };

            await _productRepo.AddAsync(product);
            await _productRepo.SaveChangesAsync();

            return new ProductResponse { Id = product.Id, Name = product.Name, Price = product.Price };
        }

        public async Task<bool> UpdateProductAsync(Guid id, CreateProductDto updateProductDto)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return false;

            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;

            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return false;

            _productRepo.Delete(product);
            await _productRepo.SaveChangesAsync();
            return true;
        }
    }
}
