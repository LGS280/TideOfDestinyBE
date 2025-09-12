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
    public class NewsRepo : INewsRepo
    {
        private TideOfDestinyDbContext _context;

        public NewsRepo(TideOfDestinyDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateNewsAsync(News news)
        {
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteNewsAsync(Guid id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null) return false;
            _context.News.Remove(news);
            _context.SaveChanges();
            return true;
        }

        public Task<List<News>> GetAllNewsAsync() => 
            _context.News
                .Include(n => n.Author)
                .OrderByDescending(n => n.PublishedAt)
                .ToListAsync();

        public Task<News?> GetNewsByIdAsync(Guid id) => 
            _context.News
                .Include(n => n.Author)
                .FirstOrDefaultAsync(n => n.Id == id);

        public async Task<bool> UpdateNewsAsync(News news)
        {
            _context.Update(news);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
