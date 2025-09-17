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
        GameFile? GetFileById(int id);
        string GetPhysicalPath(GameFile file);
    }
}
