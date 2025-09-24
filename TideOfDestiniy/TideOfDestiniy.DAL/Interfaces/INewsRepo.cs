using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.DAL.Interfaces
{
    public interface INewsRepo
    {
        Task<bool> CreateNewsAsync(News news);
        Task<News?> GetNewsByIdAsync(Guid id);
        Task<List<News>> GetAllNewsAsync(NewsCategory? newsCategory = null);
        Task<bool> UpdateNewsAsync(News news);
        Task<bool> DeleteNewsAsync(Guid id);

    }
}
