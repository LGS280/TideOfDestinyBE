using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Context;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.DAL.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly TideOfDestinyDbContext _context;

        public ProductRepo(TideOfDestinyDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetMainGameProductAsync()
        {
            // Lấy sản phẩm đầu tiên tìm thấy trong bảng.
            // Vì chỉ có 1 game, cách này là đơn giản và hiệu quả.
            var product = await _context.Products.FirstOrDefaultAsync(p => (p.Name == "TideOfDestinyGame" || p.Name == "TideOfDestiny") && p.Price == 35000);
            if (product == null)
            {
                throw new Exception("No product has been configured in the database.");
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
