using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.DAL.Interfaces
{
    public interface IFileRepo
    {
        Task AddAsync(GameFile file);
        Task<GameFile?> GetByIdAsync(int id);
        Task<GameFile?> GetByFileNameAsync(string fileName);
        Task<IEnumerable<GameFile>> GetAllAsync();
        Task<GameFile?> GetLastestFile();
    }
}
