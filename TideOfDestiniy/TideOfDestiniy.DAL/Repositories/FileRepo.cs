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

        public void Save(GameFile file)
        {
            _context.GameFiles.Add(file);
            _context.SaveChanges();
        }

        public IEnumerable<GameFile> GetAll()
        {
            return _context.GameFiles.AsNoTracking().ToList();
        }

        public GameFile? GetById(int id)
        {
            return _context.GameFiles.FirstOrDefault(f => f.Id == id);
        }
    }
}
