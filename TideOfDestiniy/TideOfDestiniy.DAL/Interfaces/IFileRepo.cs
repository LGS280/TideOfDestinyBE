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
        void Save(GameFile file);

        IEnumerable<GameFile> GetAll();
    }
}
