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
    public class FileRepo : IFileRepo
    {
        //private readonly List<GameFile> _files = new(); // giả sử DB (có thể thay bằng EF Core)
        private readonly TideOfDestinyDbContext _context;

        public FileRepo(TideOfDestinyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GameFile file)
        {
            _context.GameFiles.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task<GameFile?> GetByIdAsync(int id)
        {
            return await _context.GameFiles.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<GameFile?> GetByFileNameAsync(string fileName)
        {
            return await _context.GameFiles.FirstOrDefaultAsync(f => f.FileName == fileName);
        }

        public async Task<IEnumerable<GameFile>> GetAllAsync()
        {
            return await Task.FromResult(_context.GameFiles.ToList());
        }
    }
}
