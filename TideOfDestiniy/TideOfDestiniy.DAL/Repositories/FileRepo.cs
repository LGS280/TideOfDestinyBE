using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.DAL.Repositories
{
    public class FileRepo : IFileRepo
    {
        private readonly List<GameFile> _files = new(); // giả sử DB (có thể thay bằng EF Core)
        public IEnumerable<GameFile> GetAll()
        {
            return _files;
        }

        public void Save(GameFile file)
        {
            file.Id = _files.Count + 1;
            _files.Add(file);
        }
    }
}
