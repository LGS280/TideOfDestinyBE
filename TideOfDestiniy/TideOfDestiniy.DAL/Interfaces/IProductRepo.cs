using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.DAL.Interfaces
{
    public interface IProductRepo
    {
        Task<Product> GetMainGameProductAsync();
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<int> SaveChangesAsync();
    }
}
