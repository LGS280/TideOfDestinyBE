using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.DTOs.Responses;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponse> GetMainGameProductAsync();
        Task<IEnumerable<ProductResponse>> GetActiveProductsAsync();
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProductByIdAsync(Guid id);
        Task<ProductResponse> CreateProductAsync(CreateProductDto createProductDto);
        Task<bool> UpdateProductAsync(Guid id, CreateProductDto updateProductDto);
        Task<bool> DeleteProductAsync(Guid id);
    }
}
