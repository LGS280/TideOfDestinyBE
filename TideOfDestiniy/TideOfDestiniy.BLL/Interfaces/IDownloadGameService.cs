using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IDownloadGameService
    {
        Task<GameFile?> GetByIdAsync(int id);
        Task<Stream?> DownloadFromR2Async(GameFile file);
        Task<GameFile?> GetLastestFileAsync();
    }
}
